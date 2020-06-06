﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CustomerPath), typeof(CustomerTimer))]
public class CustomerController : MonoBehaviour, IInteractable
{
    [Tooltip("Length of path generated if the customer can't find an item.")]
    [SerializeField] int nullPathLength = 5;
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] public CustomerProfile customerProfile;
    [SerializeField] float maxRange = .001f;
    [SerializeField] public bool isWalking;
    [SerializeField] public bool drawDebugging = true;
    [SerializeField] GameObject Bulletin;

    [HideInInspector] public GameObject levelExit;
    [HideInInspector] public ShelfManager shelfManager;

    LayerMask layerMask;
    ItemInstance desiredItem;
    CustomerPath path;
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CustomerDirector customerDirector;
    CustomerTimer customerTimer;
    List<ItemInstance> unclaimedItems;
    Shelf targetShelf;
    IList<Cell> customerPath;
    string customerID;
    int pathIndex;
    float lastDirY, lastDirX;
    float customerStartTime, customerCurrentTime = 0;
    private bool isDebug;
    bool isLeaving = false;
    bool transactionReady;
    bool isWaiting;

    public bool IsLeaving
    {
        get
        { return isLeaving; }
        set
        {
            if (isLeaving == value) { return; }
            isLeaving = value;
            if(IsLeavingChange != null)
            {
                IsLeavingChange(isLeaving);
                customerTimer.IsWaiting = false;
                customerTimer.IsTransactionReady = false;
            }
        }
    }

    public bool TransactionReady
    {
        get { return transactionReady; }
        set
        {
            if(transactionReady == value) { return; }
            transactionReady = value;
            Bulletin.SetActive(transactionReady);
            if (transactionReady)
            {
                IsWaiting = false;
                customerTimer.StartTransactionTimer();
            }
            else
            {
                customerTimer.IsTransactionReady = false;
            }
        }
    }

    public bool IsWaiting
    {
        get { return isWaiting; }
        set
        {
            if (isWaiting == value) { return; }
            isWaiting = value;
            if (isWaiting)
            {
                customerTimer.StartWaitingTimer();
            }
            else
            {
                customerTimer.IsWaiting = false;
                customerDirector.WaitingCustomers.Remove(customerID);
            }
        }
    }

    public ItemInstance DesiredItem
    {
        get
        {
            return desiredItem;
        }
        set
        {
            desiredItem = value;
            customerTimer.IsWaiting = false;
            PathToItem();
        }
    }

    public Shelf TargetShelf
    {
        get
        {
            return targetShelf;
        }
        set
        {
            targetShelf = value;
            customerTimer.IsWaiting = false;
            PathToShelf();
        }
    }
    
    public delegate void OnVariableChangeDelegate(bool newVal);
    public event OnVariableChangeDelegate IsLeavingChange;

    // Start is called before the first frame update
    void Start()
    {
        path = GetComponent<CustomerPath>();
        customerDirector = FindObjectOfType<CustomerDirector>();
        customerTimer = GetComponent<CustomerTimer>();
        unclaimedItems = customerDirector.unclaimedItems;
        IsLeavingChange += IsLeavingHandler;
        customerStartTime = Time.timeSinceLevelLoad;
        layerMask = LayerMask.GetMask("Interactable", "Walls", "Player");
        Bulletin.gameObject.SetActive(false);
        pathIndex = 0;
        SetUpCustomerProfile();
        customerID = Guid.NewGuid().ToString();
        customerDirector.WaitingCustomers.Add(customerID, this);
        customerTimer.StartWaitingTimer();
    }
    
    void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// Sells the desiredItem to the customer.
    /// </summary>
    public void BuyItem()
    {
        desiredItem.Shelf.heldItems.Remove(desiredItem);
    }
    
    /// <summary>
    /// Moves the character from place to place.
    /// </summary>
    private void Move()
    {
        if (customerPath != null && pathIndex < customerPath.Count)
        {
            isWalking = true;
            TransactionReady = false;
            var target = customerPath[pathIndex].Position;
            Vector2 heading = target - transform.position;
            float distance = heading.magnitude;
            Vector2 direction = heading / distance;

            transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * walkSpeed);
            myAnimator.SetBool("IsWalking", isWalking);
            myAnimator.SetFloat("VelocityX", direction.x);
            myAnimator.SetFloat("LastXFace", direction.x);
            myAnimator.SetFloat("VelocityY", direction.y);
            myAnimator.SetFloat("LastYFace", direction.y);


            if (transform.position.Equals(target))
            {
                pathIndex++;
            }
        }
        else
        {
            if (targetShelf != null)
            {
                //If the item is not null then the character should have arrived at their destination.
                isWalking = false;
                myAnimator.SetBool("IsWalking", isWalking);
                TransactionReady = true;
            }
            else if(!isLeaving)
            {
                //If the item is null then this character has reached the end of their path and should continue wandering
                customerPath = new List<Cell>();
                GenerateWanderingPath();
            }
            else
            {
                isWalking = false;
            }
        }
    }

    //TODO: Need to rework this
    private void GenerateWanderingPath()
    {
        pathIndex = 0;
        //Get Random Shelf
        Transform shelf = GetRandomShelf();
        //Pathfind to that shelf
        path.SetEndPoints(transform.position, shelf.position);
        path.FindPathAStar();
        customerPath = path.GetPath();
    }

    private Transform GetRandomShelf()
    {
        Shelf[] children = shelfManager.GetComponentsInChildren<Shelf>();
        Shelf randomObject = children[Random.Range(0, children.Length)];
        return randomObject.standArea.transform;
    }

    private void IsLeavingHandler(bool newVal)
    {
        if (newVal == true)
        {
            if (targetShelf != null)
            {
                customerDirector.UnclaimShelf(targetShelf);
                customerDirector.RemoveFromClaimedShelves(targetShelf);
            }
            customerTimer.IsWaiting = false;
            customerTimer.IsTransactionReady = false;
            IsWaiting = false;
            customerDirector.WaitingCustomers.Remove(customerID);
            PathToExit(); 
        }
        else
        {
            Debug.Log("Customer is no longer leaving");
        }
    }
    
    private void SetUpCustomerProfile()
    {
        myAnimator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        customerDirector = FindObjectOfType<CustomerDirector>();
        myAnimator.runtimeAnimatorController = customerProfile.animatorController;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = customerProfile.customerSprite;
    }

    private void PathToItem()
    {
        Debug.Log("Pathing to new item");
        Vector3 itemLocation = desiredItem.Shelf.GetPosition();
        IsWaiting = false;
        if (pathIndex < customerPath.Count)
        {
            path.SetEndPoints(customerPath[pathIndex].Position, itemLocation);
        }
        else
        {
            path.SetEndPoints(transform.position, itemLocation);
        }
        pathIndex = 0;
        customerPath.Clear();
        path.FindPathAStar();
        customerPath = path.GetPath();
    }

    private void PathToShelf()
    {
        if (customerPath == null)
        {
            Debug.Log("Null customer path");
            customerPath = GetComponent<CustomerPath>().GetPath();
        }
        Debug.Log("Pathing to new shelf");
        Vector3 shelfLocation = targetShelf.GetPosition();
        IsWaiting = false;
        if (pathIndex < customerPath.Count)
        {
            path.SetEndPoints(customerPath[pathIndex].Position, shelfLocation);
        }
        else
        {
            path.SetEndPoints(transform.position, shelfLocation);
        }
        pathIndex = 0;
        customerPath.Clear();
        path.FindPathAStar();
        customerPath = path.GetPath();
    }

    private void PathToExit()
    {
        IsWaiting = false;
        if (desiredItem != null) { customerDirector.UnclaimItem(desiredItem); }
        if (pathIndex < customerPath.Count)
        {
            path.SetEndPoints(customerPath[pathIndex].Position, levelExit.transform.position);
        }
        else
        {
            path.SetEndPoints(transform.position, levelExit.transform.position);
        }
        pathIndex = 0;
        customerPath.Clear();
        path.FindPathAStar();
        isWalking = true;
        StartCoroutine(WaitAndDestroy());
    }

    public void Interact()
    {
        if (!isWalking)
        {
            FindObjectOfType<HagglingController>().StartHaggling(this);
        }
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitUntil(() => isWalking == false);
        Destroy(gameObject);
    }
}

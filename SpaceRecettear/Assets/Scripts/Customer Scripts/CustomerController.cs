﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerController : MonoBehaviour, IInteractable
{    
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] public CustomerProfile customerProfile;
    [SerializeField] float maxRange = .001f;
    [SerializeField] public bool isWalking;
    [SerializeField] GameObject Bulletin;

    [SerializeField] public GameObject levelExit;
    [HideInInspector] public ItemInstance desiredItem;
    [HideInInspector] public bool isFinishedShopping;

    HagglingManager hagglingManager;
    CustomerPath path;
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CustomerManager customerManager;
    List<ItemInstance> unclaimedItems;
    IList<Cell> customerPath;
    int pathIndex;
    float lastDirY;
    float lastDirX;
    private bool isDebug;

    void Awake()
    {
        hagglingManager = FindObjectOfType<HagglingManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Bulletin.gameObject.SetActive(false);
        pathIndex = 0;
        SetUpCustomerProfile();
        path = GetComponent<CustomerPath>();
        FindNewItem();
    }

    // Update is called once per frame
    void Update()
    {
        if(desiredItem == null) { FindNewItem(); }
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

    public void GoToExit()
    {
        pathIndex = 0;
        customerPath.Clear();
        path.SetEndPoints(transform.position, levelExit.transform.position);
        path.FindPathAStar();
        isWalking = true;
    }

    /// <summary>
    /// Moves the character from place to place.
    /// </summary>
    private void Move()
    {
        if (customerPath != null && pathIndex < customerPath.Count)
        {
            isWalking = true;
            SetReady(false);
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
            isWalking = false;
            myAnimator.SetBool("IsWalking", isWalking);
            SetReady(true);
        }

        if(customerPath == null)
        {
            Debug.LogError(gameObject.name + "path is null for some reason. Destroying Customer.");
            Destroy(gameObject);//TODO: Make them wander instead?
        }
    }

    private void SetReady(bool status)
    {
        Bulletin.SetActive(status);
    }

    private void SetUpCustomerProfile()
    {
        myAnimator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        customerManager = FindObjectOfType<CustomerManager>();
        myAnimator.runtimeAnimatorController = customerProfile.animatorController;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = customerProfile.customerSprite;
    }

    private void FindNewItem()
    {
        unclaimedItems = customerManager.unclaimedItems;
        //TODO: I feel customers should be choosing from a list of preferred items, but for now I'm just choosing randomly from shelved things
        if (unclaimedItems.Count > 0)
        {
            //TODO later, I plan to change this part right here to check for a random item from prefered items. 
            int itemIndex = Random.Range(0, unclaimedItems.Count);
            desiredItem = unclaimedItems[itemIndex];//For now it's just getting a random item on the shelves and calling the method I've placed for later.
            desiredItem = customerManager.ClaimItem(desiredItem);//TODO need to decide what to do if this returns null.

            Vector3 itemLocation = desiredItem.Shelf.GetPosition();
            path.SetEndPoints(transform.position,itemLocation);
            path.FindPathAStar();

            customerPath = path.GetPath();
            return;
        }
        desiredItem = null;
    }

    public void Interact()
    {
        if (!isWalking)
        {
            hagglingManager.SetActiveCustomer(gameObject);
            hagglingManager.SetItemForSale(desiredItem);
            hagglingManager.ShowCanvas();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Exit") && isFinishedShopping)
        {
            StartCoroutine(WaitAndDestroy());
        }
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitUntil(() => isWalking == false);
        Destroy(gameObject);
    }
}

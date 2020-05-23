using System;
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

    [SerializeField] public GameObject levelExit;
    [HideInInspector] public ItemInstance desiredItem;
    [HideInInspector] public bool isFinishedShopping;

    LayerMask layerMask;
    CustomerPath path;
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CustomerManager customerManager;
    CustomerTimer customerTimer;
    List<ItemInstance> unclaimedItems;
    IList<Cell> customerPath;
    int pathIndex;
    float lastDirY, lastDirX;
    float customerStartTime, customerCurrentTime = 0;
    private bool isDebug;
    bool isLeaving;
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
                customerTimer.StopWaitingTimer();
                customerTimer.StopTransactionTimer();
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
                customerTimer.StopWaitingTimer();
                customerTimer.StartTransactionTimer();
            }
            else
            {
                customerTimer.StopTransactionTimer();
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
                customerTimer.StopWaitingTimer();
            }
        }
    }

    public delegate void OnVariableChangeDelegate(bool newVal);
    public event OnVariableChangeDelegate IsLeavingChange;

    // Start is called before the first frame update
    void Start()
    {
        customerManager = FindObjectOfType<CustomerManager>();
        path = GetComponent<CustomerPath>();
        customerTimer = GetComponent<CustomerTimer>();
        unclaimedItems = customerManager.unclaimedItems;
        IsLeavingChange += IsLeavingHandler;
        customerStartTime = Time.timeSinceLevelLoad;
        layerMask = LayerMask.GetMask("Interactable", "Walls", "Player");
        Bulletin.gameObject.SetActive(false);
        pathIndex = 0;
        SetUpCustomerProfile();
        FindNewItem();
    }

    // Update is called once per frame
    void Update()
    {
        if(desiredItem == null)
        {
            FindNewItem();
        }
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
            if (desiredItem != null)
            {
                //If the item is not null then the character should have arrived at their destination.
                isWalking = false;
                myAnimator.SetBool("IsWalking", isWalking);
                TransactionReady = true;
            }
            else
            {
                //If the item is null then this character has reached the end of their path and should continue wandering
                customerPath = new List<Cell>();
                customerPath = GenerateWanderingPath();
                IsWaiting = true;
            }
        }
    }

    //TODO: Need to rework this
    private IList<Cell> GenerateWanderingPath()
    {
        pathIndex = 0;
        List<Cell> newPath = new List<Cell>();
        return newPath;
    }

    private void IsLeavingHandler(bool newVal)
    {
        if (newVal == true)
        {
            if (desiredItem != null) { unclaimedItems.Add(desiredItem); }
            customerTimer.StopWaitingTimer();
            customerTimer.StopTransactionTimer();
            PathToExit(); 
        }
        else
        {
            Debug.Log("Customer is no longer leaving");
        }
    }

    private void PathToExit()
    {
        pathIndex = 0;
        customerPath.Clear();
        path.SetEndPoints(transform.position, levelExit.transform.position);
        path.FindPathAStar();
        isWalking = true;
        StartCoroutine(WaitAndDestroy());
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
            FindObjectOfType<HagglingController>().StartHaggling(this);
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

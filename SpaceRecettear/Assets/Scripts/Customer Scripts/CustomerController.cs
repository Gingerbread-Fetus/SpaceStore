using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
        layerMask = LayerMask.GetMask("Interactable", "Walls");
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
            if (desiredItem != null)
            {
                isWalking = false;
                myAnimator.SetBool("IsWalking", isWalking);
                SetReady(true); 
            }
            else
            {
                customerPath = new List<Cell>();
                customerPath = GenerateWanderingPath();
            }
        }

        if(customerPath == null)
        {
            SetReady(false);
            Debug.LogError(gameObject.name + "path is null for some reason. Wandering Customer.");
            customerPath = GenerateWanderingPath();
            isWalking = true;
        }
    }

    private IList<Cell> GenerateWanderingPath()
    {
        pathIndex = 0;
        List<Cell> newPath = new List<Cell>();
        Cell startingCell = new Cell(transform.position, transform.position);//The heuristic won't matter for this case, so we can just ignore the goal.
        Cell currentCell = startingCell;
        for(int i = 0; i < nullPathLength; i++)
        {
            Cell nextCell = GetRandomNeighbor(currentCell);
            newPath.Add(nextCell);
        }
        return newPath;
    }

    private Cell GetRandomNeighbor(Cell currentCell)
    {
        List<Cell> neighbors = new List<Cell>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3 tmp = new Vector3(x, y, 0);
                Vector3 nextPos = tmp + currentCell.Position;
                if (x == 0 && y == 0) { continue; }//skip over the 'center' cell.
                Cell nextSuccessor = new Cell(currentCell, nextPos, nextPos);////for neighbors of current: cost = g(current) + movementcost(current, neighbor)
                RaycastHit2D hit = Physics2D.Raycast(currentCell.Position, (nextSuccessor.Position - currentCell.Position).normalized, 1f, layerMask);

                if (hit)//If fraction <= 0 then the collision came from inside the collider.
                {
                    //Debug.Log(hit + ": hit detected by raycast on " + hit.collider.gameObject.name);
                    continue;
                }
                else
                {
                    if (drawDebugging) { Debug.DrawRay(nextSuccessor.Position, Vector3.right * .5f, Color.yellow, 3.0f); }
                    nextSuccessor.parent = currentCell;
                    neighbors.Add(nextSuccessor);
                }
            }
        }
        return neighbors[Random.Range(0,neighbors.Count)];
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

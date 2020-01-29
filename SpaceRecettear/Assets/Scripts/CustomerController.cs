using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour, IInteractable
{    
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] public ItemInstance desiredItem;
    [SerializeField] public CustomerProfile customerProfile;

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

    // Start is called before the first frame update
    void Start()
    {
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
    /// TODO implement and test this
    /// </summary>
    public void BuyItem()
    {
        desiredItem.Shelf.heldItems.Remove(desiredItem);
    }

    /// <summary>
    /// TODO: Customers move between cells instantly, needs to be slowed down.
    /// </summary>
    private void Move()
    {

        bool playerIsMoving = myRigidBody.velocity.magnitude > Mathf.Epsilon;
        myAnimator.SetBool("IsWalking", playerIsMoving);

        if (customerPath != null && pathIndex < customerPath.Count)//-1?
        {
            var target = customerPath[pathIndex].Position;
            Vector2 direction = (target - transform.position).normalized;
            Vector2.Lerp(target, direction,1);


            if (transform.position == target)
            {
                pathIndex++;
            }
        }

        if(customerPath == null)
        {
            Debug.LogError(gameObject.name + "path is null for some reason. Destroying Customer.");
            Destroy(gameObject);//TODO: Make sure you delete this later.
        }

        float verticalAxis = myRigidBody.velocity.y;
        float horizontalAxis = myRigidBody.velocity.x;

        lastDirX = horizontalAxis;
        lastDirY = verticalAxis;

        //set animator parameters
        myAnimator.SetFloat("VelocityX", horizontalAxis);
        myAnimator.SetFloat("LastXFace", lastDirX);
        myAnimator.SetFloat("VelocityY", verticalAxis);
        myAnimator.SetFloat("LastYFace", lastDirY);
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
        Debug.Log("Customer interacted with.");
    }
}

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
    ShelfManager shelves;
    List<ItemInstance> shelvedItems;
    float lastDirY;
    float lastDirX;
    private bool isDebug;

    // Start is called before the first frame update
    void Start()
    {
        SetUpCustomerProfile();
        path = GetComponent<CustomerPath>();
        FindNewItem();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if(desiredItem == null) { FindNewItem(); }
    }

    /// <summary>
    /// TODO: This was copied from the player movement script. It will need to 
    /// be changed. Probably changed to not use the physics velocity, make sure
    /// that whatever is used is updating the animator parameters accurately.
    /// </summary>
    private void Move()
    {
        float verticalAxis = -1f;
        float horizontalAxis = 0f;
        
        lastDirX = horizontalAxis;
        lastDirY = verticalAxis;
        
        //set animator parameters
        myAnimator.SetFloat("VelocityX", horizontalAxis);
        myAnimator.SetFloat("LastXFace", lastDirX);
        myAnimator.SetFloat("VelocityY", verticalAxis);
        myAnimator.SetFloat("LastYFace", lastDirY);
        //move player
        myRigidBody.velocity = new Vector2(
            horizontalAxis * walkSpeed * Time.deltaTime,
            verticalAxis * walkSpeed * Time.deltaTime
            );

        bool playerIsMoving = myRigidBody.velocity.magnitude > Mathf.Epsilon;
        myAnimator.SetBool("IsWalking", playerIsMoving);
    }

    private void SetUpCustomerProfile()
    {
        myAnimator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        shelves = FindObjectOfType<ShelfManager>();
        shelvedItems = shelves.GetShelvedItems();
        myAnimator.runtimeAnimatorController = customerProfile.animatorController;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = customerProfile.customerSprite;
    }

    private void FindNewItem()
    {
        //TODO: I feel customers should be choosing from a list of preferred items, but for now I'm just choose randomly from shelved things
        if (shelvedItems.Count > 0)
        {
            Debug.Log("Finding item");
            int itemIndex = Random.Range(0, shelvedItems.Count);
            desiredItem = shelvedItems[itemIndex];
            Vector3 itemLocation = desiredItem.shelf.transform.position;
            path.SetEndingPosition(itemLocation);

            path.FindPathAStar();
            return;
        }
        desiredItem = null;
    }

    public void Interact()
    {
        Debug.Log("Customer interacted with.");
    }
}

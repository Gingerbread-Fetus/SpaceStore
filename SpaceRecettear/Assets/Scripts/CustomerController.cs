﻿using System.Collections.Generic;
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

        bool playerIsMoving = myRigidBody.velocity.magnitude > Mathf.Epsilon;
        myAnimator.SetBool("IsWalking", playerIsMoving);

        if (pathIndex < customerPath.Count)//-1?
        {
            var targetPosition = customerPath[pathIndex].Position;
            myRigidBody.MovePosition(targetPosition);

            if(transform.position == targetPosition)
            {
                pathIndex++;
            }
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

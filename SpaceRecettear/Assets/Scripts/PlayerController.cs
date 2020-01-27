using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed = 5f;

    Rigidbody2D myRigidBody;
    Animator myAnimator;
    LayerMask interactableLayerMask;
    float lastDirY;
    float lastDirX;
    private bool isDebug;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        interactableLayerMask = LayerMask.GetMask("Interactable", "Customers"); 
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        PlayerInteract();
        DrawDebug();
    }

    private void DrawDebug()
    {

        if (Input.GetButtonDown("Debug"))
        {
            Debug.Log("Debug pressed");
            isDebug = !isDebug;
        }

        if (isDebug)
        {
            Vector3 rayDir = new Vector3(lastDirX, lastDirY, 0).normalized;
            Debug.DrawLine(transform.position, rayDir);
            Debug.DrawRay(transform.position, rayDir, Color.red);
        }
    }

    private void PlayerInteract()
    {
        if (Input.GetButtonDown("Interact"))
        {
            //On interact, raycast to first object in facing direction
            Vector2 facingDirection = new Vector2(lastDirX, lastDirY);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDirection, 1, interactableLayerMask);
            
            if(hit)
            {
                //If an object is hit, check if it's interactable. Should be if it's on that layer
                IInteractable interactable = hit.transform.gameObject.GetComponent<IInteractable>();
                if(interactable != null)
                {
                    interactable.Interact();
                }
                    
            }
        }
    }

    private void Move()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");

        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            lastDirX = horizontalAxis;
            lastDirY = verticalAxis;
        }
        //set animator parameters
        myAnimator.SetFloat("InputX", horizontalAxis);
        myAnimator.SetFloat("LastXFace", lastDirX);
        myAnimator.SetFloat("InputY", verticalAxis);
        myAnimator.SetFloat("LastYFace", lastDirY);
        //move player
        myRigidBody.velocity = new Vector2(horizontalAxis * walkSpeed * Time.deltaTime, verticalAxis * walkSpeed * Time.deltaTime);

        bool playerIsMoving = myRigidBody.velocity.magnitude > Mathf.Epsilon;
        myAnimator.SetBool("Moving", playerIsMoving); 
        
    }
}

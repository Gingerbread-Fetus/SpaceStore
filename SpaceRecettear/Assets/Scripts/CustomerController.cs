using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    [SerializeField] float walkSpeed = 5f;

    Rigidbody2D myRigidBody;
    Animator myAnimator;
    LayerMask interactableLayerMask;
    float lastDirY;
    float lastDirX;
    private bool isDebug;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

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
        myRigidBody.velocity = new Vector2(horizontalAxis * walkSpeed * Time.deltaTime, verticalAxis * walkSpeed * Time.deltaTime);

        bool playerIsMoving = myRigidBody.velocity.magnitude > Mathf.Epsilon;
        myAnimator.SetBool("IsWalking", playerIsMoving);
    }
}

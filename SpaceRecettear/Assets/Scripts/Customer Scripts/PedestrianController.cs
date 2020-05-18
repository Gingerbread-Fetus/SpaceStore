using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PedestrianController : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float respawnTimeLimit = 1.0f;
    [SerializeField] List<CustomerProfile> customerProfiles;

    Animator myAnimator;
    Rigidbody2D myRigidBody;
    CustomerProfile myCustomerProfile;
    bool isWalking;
    [SerializeField] Vector3 target;
    Vector2 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        startPosition = transform.position;
        ChangeProfile();
        isWalking = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (isWalking == true)
        {
            Vector2 heading = target - transform.position;
            float distance = heading.magnitude;
            Vector2 direction = heading / distance;

            transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * walkSpeed);
            myAnimator.SetBool("IsWalking", isWalking);
            myAnimator.SetFloat("VelocityX", direction.x);
            myAnimator.SetFloat("LastXFace", direction.x);
            myAnimator.SetFloat("VelocityY", direction.y);
            myAnimator.SetFloat("LastYFace", direction.y); 
        }
    }

    public void SetGoal(GameObject newGoal)
    {
        target = new Vector2(transform.position.x, newGoal.transform.position.y);
        isWalking = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isWalking = false;
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(Random.Range(0, respawnTimeLimit));
        transform.position = startPosition;
        ChangeProfile();
        isWalking = true;
    }

    private void ChangeProfile()
    {
        myAnimator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        myCustomerProfile = customerProfiles[Random.Range(0, customerProfiles.Count)];
        myAnimator.runtimeAnimatorController = myCustomerProfile.animatorController;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = myCustomerProfile.customerSprite;
    }
}

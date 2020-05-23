using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerTimer : MonoBehaviour
{
    [Tooltip("How long the customer will walk around the store before leaving.")] [SerializeField] float customerSearchTimeout = 5f;
    [Tooltip("How long the customer will wait on a transaction before leaving.")] [SerializeField] float customerTransactionTimeout = 5f;
    CustomerController customerController;
    float transactionTime, transactionStartTime, waitTime, waitStartTime = 0f;
    bool isWaiting, isTransactionReady = false;


    void Start()
    {
        waitTime = 0f;
        customerController = GetComponent<CustomerController>();
        transactionStartTime = Time.timeSinceLevelLoad;
        waitStartTime = Time.timeSinceLevelLoad;
    }

    void Update()
    {
        if (isWaiting)
        {
            waitTime = Time.timeSinceLevelLoad - waitStartTime;
            CheckWaitTimer();
        }
        if (isTransactionReady)
        {
            transactionTime = Time.timeSinceLevelLoad - transactionStartTime;
            CheckTransactionTimer();
        }
    }

    public void StartTransactionTimer()
    {
        transactionTime = 0f;
        transactionStartTime = Time.timeSinceLevelLoad;
        isTransactionReady = true;
    }

    public void StartWaitingTimer()
    {
        waitTime = 0f;
        waitStartTime = Time.timeSinceLevelLoad;
        isWaiting = true;
    }

    public void StopTransactionTimer()
    {
        isTransactionReady = false;
    }

    public void StopWaitingTimer()
    {
        isWaiting = false;
    }

    public void ResumeTransactionTimer()
    {
        isTransactionReady = true;
    }

    public void ResumeWaitingTimer()
    {
        isWaiting = true;
    }

    private void CheckTransactionTimer()
    {
        if(transactionTime > customerTransactionTimeout)
        {
            Debug.Log("Customer " + gameObject.name + " transaction has timed out, they are leaving.");
            isTransactionReady = false;
            customerController.IsLeaving = true;
        }
    }

    private void CheckWaitTimer()
    {
        if(waitTime > customerTransactionTimeout)
        {
            Debug.Log("Customer " + gameObject.name + " didn't find anything, they are leaving.");
            isWaiting = false;
            customerController.IsLeaving = true;
        }
    }
}

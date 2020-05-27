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

    public float TransactionTime { get => transactionTime; set => transactionTime = value; }
    public float WaitTime { get => waitTime; set => waitTime = value; }
    public bool IsWaiting { get => isWaiting; set => isWaiting = value; }
    public bool IsTransactionReady { get => isTransactionReady; set => isTransactionReady = value; }

    void Start()
    {
        WaitTime = 0f;
        customerController = GetComponent<CustomerController>();
        transactionStartTime = Time.timeSinceLevelLoad;
        waitStartTime = Time.timeSinceLevelLoad;
    }

    void Update()
    {
        if (IsWaiting)
        {
            WaitTime = Time.timeSinceLevelLoad - waitStartTime;
            CheckWaitTimer();
        }
        if (IsTransactionReady)
        {
            TransactionTime = Time.timeSinceLevelLoad - transactionStartTime;
            CheckTransactionTimer();
        }
    }

    public void StartTransactionTimer()
    {
        TransactionTime = 0f;
        transactionStartTime = Time.timeSinceLevelLoad;
        IsTransactionReady = true;
    }

    public void StartWaitingTimer()
    {
        WaitTime = 0f;
        waitStartTime = Time.timeSinceLevelLoad;
        IsWaiting = true;
    }
    
    private void CheckTransactionTimer()
    {
        if(TransactionTime > customerTransactionTimeout)
        {
            Debug.Log("Customer " + customerController.customerProfile.characterName + " transaction has timed out, they are leaving.");
            IsTransactionReady = false;
            customerController.IsLeaving = true;
        }
    }

    private void CheckWaitTimer()
    {
        if(WaitTime > customerTransactionTimeout)
        {
            Debug.Log("Customer " + customerController.customerProfile.characterName + " didn't find anything, they are leaving.");
            IsWaiting = false;
            customerController.IsLeaving = true;
        }
    }
}

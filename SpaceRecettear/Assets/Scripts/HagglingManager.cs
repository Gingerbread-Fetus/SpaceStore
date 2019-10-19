using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HagglingManager handles turns in the haggling screen.
/// </summary>
public class HagglingManager : MonoBehaviour
{
    private bool introShown;
    [SerializeField] float gameSpeed;
    [SerializeField] bool playerTurn;
    [SerializeField] StoreInventory playerInventory;
    [SerializeField] StoreInventory customerInventory;
    // Start is called before the first frame update
    void Start()
    {
         //GetCustomer()//Don't know if I should do this, or just set the customer when the encounter manager finds one.
    }

    void OnEnable()
    {
        PrintIntro();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerTurn)
        {
            HandleCustomerTurn();
        }
    }

    private void HandleCustomerTurn()
    {
        Debug.Log("Customer turn");
        playerTurn = true;
        Debug.Log("Ending customer turn, now player turn.");
    }

    private void HandlePlayerTurn()
    {
        while (playerTurn)
        {
            //Maybe kinda needless, but I want this here as a placeholder for anything I want to do while it's the player's turn.
        }
    }

    public void Haggle()
    {
        if (playerTurn)
        {
            //The important part is that these methods end the turn
            Debug.Log("Player has made a move ending turn");
            playerTurn = false; 
        }
    }

    private void PrintIntro()
    {
        //Play customer intro text
        introShown = true;
    }
}

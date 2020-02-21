using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// The customer profile class provides a base I can expand on later to define different
/// decision structures for other customers!
/// TODO: Implement different types of these for the different customers.
/// </summary>
[CreateAssetMenu(fileName = "New Customer Profile", menuName = "Customers/Profile")]
public class CustomerProfile : ScriptableObject
{
    [SerializeField] public float saleThreshold = 0;
    [SerializeField] public int lowThreshold = 10;
    [SerializeField] public int highThreshold = 40;
    [SerializeField] public List<ItemInstance> favoriteItems;
    [SerializeField] public List<ItemInstance> hatedItems;
    [SerializeField] public Sprite customerSprite;
    [SerializeField] public AnimatorOverrideController animatorController;
    int likedItemNumber = 0;
    int desiredPrice;
    int pricePercentageInt;
    private Transaction currentTransaction;
    
    /// <summary>
    /// This method checks the favorability of a transaction, if it meets the
    /// threshold then it will return true, representing a successful sale for
    /// the player. 
    /// </summary>
    /// <param name="currentTransaction"></param>
    /// <returns></returns>
    public bool ProcessTransaction()
    {
        return CalculateFavorability() <= saleThreshold;
    }

    public void SetTransaction(Transaction newTransaction)
    {
        currentTransaction = newTransaction;
        if (currentTransaction != null)
        {
            CalculateDesiredPrice();
        }
    }

    public float CalculateFavorability()
    {
        int transactionDifference = currentTransaction.Offer - desiredPrice;
        if(transactionDifference <= 0)
        {
            Debug.Log("Desired Price: " + desiredPrice.ToString() + ", Favorability: " + ((float)transactionDifference / (float)currentTransaction.GetValue()).ToString() + ",  Sale Threshold: " + saleThreshold);
            return -10;
        }
        Debug.Log("Desired Price: " + desiredPrice.ToString() + ", Favorability: " + ((float)transactionDifference / (float)currentTransaction.GetValue()).ToString() + ",  Sale Threshold: " + saleThreshold);
        return (float)transactionDifference / (float)currentTransaction.GetValue();
    }

    public int HaggleTransaction(Transaction currentTransaction)
    {
        int offerChange = -1;
        //If the current offer is less than the or equal to value of the transaction, do nothing
        if (currentTransaction.Offer <= currentTransaction.GetValue())
        {
            return 0;
        }
        else//else the current offer is > the value of the transaction, they'd overpay, attempt to lower it. 
        {
            offerChange = (currentTransaction.GetValue() - currentTransaction.Offer)/2;
            return offerChange;
        }
    }

    private void CalculateDesiredPrice()
    {
        foreach (ItemInstance itemInstance in currentTransaction.offeredItems)
        {
            if (hatedItems.Contains(itemInstance) && likedItemNumber > -10)
            {
                likedItemNumber--;
            }
            else if (favoriteItems.Contains(itemInstance) && likedItemNumber < 10)
            {
                likedItemNumber++;
            }
        }

        pricePercentageInt = Random.Range(lowThreshold, highThreshold);
        pricePercentageInt += likedItemNumber;
        saleThreshold = (float)pricePercentageInt / 100f;
        Debug.Log("Freshly calculated sale threshold: " + saleThreshold.ToString());
        float pricePercentage = 1 + ((float)pricePercentageInt / 100);
        desiredPrice = (int)Math.Round(pricePercentage * currentTransaction.GetValue());
        Debug.Log("Freshly calculated desired Price: " + desiredPrice.ToString());
    }
}

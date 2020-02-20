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
    [SerializeField] public int saleThreshold = 0;
    [SerializeField] public int lowThreshold = 10;
    [SerializeField] public int highThreshold = 40;
    [SerializeField] public List<ItemInstance> favoriteItems;
    [SerializeField] public List<ItemInstance> hatedItems;
    [SerializeField] public Sprite customerSprite;
    [SerializeField] public AnimatorOverrideController animatorController;
    int favorabilityRating = 0;
    int likedItemNumber = 0;
    int desiredPrice;

    public int FavorabilityRating { get => favorabilityRating;}

    /// <summary>
    /// This method checks the favorability of a transaction, if it meets the
    /// threshold then it will return true, representing a successful sale for
    /// the player. 
    /// </summary>
    /// <param name="currentTransaction"></param>
    /// <returns></returns>
    public bool ProcessTransaction(Transaction currentTransaction)
    {
        return CalculateFavorability(currentTransaction) >= saleThreshold;
    }

    public float CalculateFavorability(Transaction currentTransaction)
    {
        int transactionDifference = CalculateTransactionDifference(currentTransaction);
        if(transactionDifference <= 0)
        {
            return 10;
        }
        return transactionDifference / currentTransaction.GetValue();
        Debug.Log("FavorabilityRating: " + favorabilityRating.ToString() + ",  Sale Threshold: " + saleThreshold);
        return favorabilityRating;
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

    private int CalculateTransactionDifference(Transaction currentTransaction)
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
        int pricePercentageInt = Random.Range(lowThreshold, highThreshold);
        pricePercentageInt += likedItemNumber;
        float pricePercentage = 1 + ((float)pricePercentageInt / 100);
        desiredPrice = (int)pricePercentage * currentTransaction.GetValue();
        return currentTransaction.Offer - desiredPrice;
    }
}

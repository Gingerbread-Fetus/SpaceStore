using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The customer profile class provides a base I can expand on later to define different
/// decision structures for other customers!
/// TODO: Implement different types of these for the different customers.
/// </summary>
[CreateAssetMenu(fileName = "New Customer Profile", menuName = "Customers/Profile")]
public class CustomerProfile : ScriptableObject
{
    [SerializeField] public int saleThreshold = 0;
    [SerializeField] public List<ItemInstance> favoriteItems;
    [SerializeField] public List<ItemInstance> hatedItems;
    [SerializeField] public Sprite customerSprite;
    [SerializeField] public AnimatorOverrideController animatorController;
    int favorabilityRating = 0;

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

    public int CalculateFavorability(Transaction currentTransaction)
    {
        favorabilityRating = 0;
        foreach (ItemInstance itemInstance in currentTransaction.offeredItems)
        {
            if (hatedItems.Contains(itemInstance))
            {
                favorabilityRating--;
            }
            else if (favoriteItems.Contains(itemInstance))
            {
                favorabilityRating++;
            }
        }
        int transactionWeight = GetTransactionWeight(currentTransaction);
        favorabilityRating += transactionWeight;
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

    private int GetTransactionWeight(Transaction currentTransaction)
    {
        int transactionWeight = currentTransaction.GetValue() - currentTransaction.Offer;
        float approvalPercentage = (float)transactionWeight / (float)currentTransaction.Offer;
        double roundedWeight = Math.Round(approvalPercentage, 1);
        int finalWeight = (int)(roundedWeight * 10);
        if (finalWeight >= 10)
        {
            return 10;
        }
        else return finalWeight;
    }
}

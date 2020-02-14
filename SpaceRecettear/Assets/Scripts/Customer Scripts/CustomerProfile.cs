using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Customer Profile", menuName = "Customers/Profile")]
public class CustomerProfile : ScriptableObject
{
    [SerializeField] public int saleThreshold = 10;
    [SerializeField] public List<ItemInstance> favoriteItems;
    [SerializeField] public List<ItemInstance> hatedItems;
    [SerializeField] public Sprite customerSprite;
    [SerializeField] public AnimatorOverrideController animatorController;
        
    /// <summary>
    /// This method checks the favorability of a transaction, if it meets the
    /// threshold then it will return true, representing a successful sale for
    /// the player. 
    /// </summary>
    /// <param name="currentTransaction"></param>
    /// <returns></returns>
    public bool ProcessTransaction(Transaction currentTransaction)
    {
        int favorabilityRating = 0;
        foreach(ItemInstance itemInstance in currentTransaction.offeredItems)
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
        return favorabilityRating >= saleThreshold;
    }

    private int GetTransactionWeight(Transaction currentTransaction)
    {
        int transactionWeight = currentTransaction.GetValue() - currentTransaction.Offer;
        transactionWeight = transactionWeight * transactionWeight * transactionWeight;
        if (transactionWeight >= 10)
        {
            return 10;
        }
        else return transactionWeight;
    }
}

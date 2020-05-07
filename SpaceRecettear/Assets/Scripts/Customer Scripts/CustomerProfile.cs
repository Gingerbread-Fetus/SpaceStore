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
    [SerializeField][Tooltip("Percentage of desired price that will decrease desirability ")] float priceFalloff = 0.03F;
    [SerializeField] public int lowThreshold = 10;
    [SerializeField] public int highThreshold = 40;
    [SerializeField] public List<ItemInstance> favoriteItems;
    [SerializeField] public List<ItemInstance> hatedItems;
    [SerializeField] public Sprite customerSprite;
    [SerializeField] public AnimatorOverrideController animatorController;
    public string characterName;
    public int characterLevel = 1;

    QuestInfo assignedQuest;
    int likedItemNumber = 0;
    int desiredPrice;
    int pricePercentageInt;

    public float PriceFalloff { get => priceFalloff;}

    public void AssignQuest(QuestInfo newQuest)
    {
        assignedQuest = newQuest;
        Debug.Log("Adventurer: " + characterName + "Assigned to: " + newQuest.questName);
        //todo Do other things when quest is assigned?
        //Otherwise we can probably just make this variable public/exposed.
    }

    public QuestInfo GetAssignedQuest()
    {
        return assignedQuest;
    }
    
    public int CalculateDesiredPrice(int valueOfItems)
    {
        int desiredPriceRange = Random.Range(lowThreshold, highThreshold);
        float desiredPricePercentage = (float)desiredPriceRange / 100f;
        float itemPriceFloat = valueOfItems + (valueOfItems * (desiredPricePercentage));
        desiredPrice = Mathf.RoundToInt(itemPriceFloat);
        return desiredPrice;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HagglingController : MonoBehaviour
{
    [SerializeField] HagglingCanvas hagglingCanvas;
    [SerializeField] StoreInventory playerInventory;
    [SerializeField] PlayerProfile playerProfile;
    [SerializeField] int hagglingCost = 10;
    
    [HideInInspector]public Transaction transaction;
    [HideInInspector]public int currentOffer = 0;

    int currentStamina;
    int satisfactionLevel = 4;
    int satisfactionMax = 4;
    int desiredPrice = 0;
    CustomerController activeCustomerController;
    CustomerProfile activeCustomerProfile;
    ItemInstance desiredItem;

    public ItemInstance DesiredItem { get => desiredItem;}
    public StoreInventory PlayerInventory { get => playerInventory;}
    public PlayerProfile PlayerProfile { get => playerProfile;}
    public int CurrentStamina { get => currentStamina; set => currentStamina = value; }
    public int HagglingCost { get => hagglingCost; }
    public int SatisfactionLevel { get => satisfactionLevel; set => satisfactionLevel = value; }
    public int SatisfactionMax { get => satisfactionMax; set => satisfactionMax = value; }

    private void Start()
    {
        CreateMiniGames();
    }

    public void StartHaggling(CustomerController customerController)
    {
        transaction = new Transaction();
        hagglingCanvas.gameObject.SetActive(true);
        activeCustomerController = customerController;
        activeCustomerProfile = customerController.customerProfile;
        desiredItem = activeCustomerController.DesiredItem;

        Debug.Log("Active Customer: " + activeCustomerProfile.characterName +
            ", Desired Item: " + desiredItem.item.name);

        transaction.AddItem(activeCustomerController.DesiredItem);
        transaction.Offer = desiredItem.item.baseSellPrice;
        desiredPrice = activeCustomerProfile.CalculateDesiredPrice(transaction.GetValue());
        Debug.Log("desired price: " + desiredPrice);
        CalculateSatisfaction();
        CurrentStamina = playerProfile.GetStamina();

        hagglingCanvas.RedrawAll();
    }

    public void CalculateSatisfaction()
    {
        int transactionValue = transaction.GetValue();
        int transactionOfferPriceDifference = desiredPrice - transaction.Offer;
        int falloffRate = Mathf.RoundToInt(activeCustomerProfile.PriceFalloff * transaction.GetValue());
        Debug.Log("Offer Falloff: " + falloffRate);
        int falloffDelta = transactionOfferPriceDifference / falloffRate;
        Debug.Log("Falloff Delta: " + falloffDelta);
        satisfactionLevel = falloffDelta + satisfactionMax;
    }

    public void NoDeal()
    {
        hagglingCanvas.gameObject.SetActive(false);
        FindObjectOfType<CustomerDirector>().UnclaimItem(desiredItem);
        activeCustomerController.IsLeaving = true;
    }

    public void Sell()
    {
        if (satisfactionLevel > 0)
        {
            Debug.Log("Transaction Approved.");
            desiredItem.Shelf.heldItems.Remove(desiredItem);
            playerInventory.AddCurrency(transaction.Offer);
            transaction.ClearTransaction();
            hagglingCanvas.gameObject.SetActive(false);
            activeCustomerController.IsLeaving = true;
            Debug.Log(playerInventory.GetCurrency());
        }
        else
        {
            Debug.Log("Not good enough!");
        }
    }

    private void CreateMiniGames()
    {
        foreach(Ability ability in PlayerProfile.Abilities)
        {
            var abilityObject = ability.Initialize();
            abilityObject.transform.parent = this.transform;
            hagglingCanvas.CreateAbilityButton(ability, abilityObject);
        }
    }

    public void AddItemToTransaction(ItemInstance newItem)
    {
        transaction.AddItem(newItem);
        var newObject = Instantiate(new GameObject(), hagglingCanvas.ItemPanel.transform, false);
        var newImage = newObject.AddComponent<Image>();
        newObject.name = newItem.item.name;
        newImage.sprite = newItem.item.itemIcon;
    }
}

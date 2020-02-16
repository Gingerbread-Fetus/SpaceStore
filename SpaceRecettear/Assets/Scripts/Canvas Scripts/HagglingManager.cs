using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct Transaction
{
    [SerializeField] public List<ItemInstance> offeredItems;
    int totalValue;
    int offer;

    public int Offer { get => offer; set => offer = value; }

    public void AddItem(ItemInstance newItem)
    {
        offeredItems.Add(newItem);
        totalValue = totalValue += newItem.item.baseSellPrice;
    }

    public void ClearTransaction()
    {
        offeredItems.Clear();
        Offer = 0;
        totalValue = 0;
    }

    public int GetValue()
    {
        return totalValue;
    }
}

/// <summary>
/// HagglingManager handles turns in the haggling screen.
/// </summary>
public class HagglingManager : MonoBehaviour
{
    private bool introShown;
    private bool isAbilitiesVisible = false;
    OfferChanger offerChanger;
    private ItemInstance itemForSale;
    [SerializeField]private CustomerController activeCustomer;//Serialized for debugging
    private CustomerProfile activeCustomerProfile;

    [Header("Game Session Properties")]
    [SerializeField] PlayerController playerController;
    [SerializeField] bool isCanvasActive;
    [SerializeField] float gameSpeed;
    [SerializeField] int stamina = 50;
    [HideInInspector] public int currentOffer;
    [SerializeField] bool playerTurn;
    [Header("Inventory references")]
    [SerializeField] StoreInventory playerInventory;
    [SerializeField] PlayerProfile playerProfile;
    [SerializeField] StoreInventory customerInventory;
    [Header("Haggling Canvas References")]
    [SerializeField] GameObject offeredItemsCanvas;
    [SerializeField] GameObject hagglingCanvas;
    [SerializeField] GameObject abilityPanel;
    [SerializeField] GameObject hagglePanel;
    [SerializeField] SatisfactionBubble satisfactionBubble;
    [SerializeField] Image imageHolder;
    [SerializeField] TextMeshProUGUI offerText;
    [SerializeField] TextMeshProUGUI transactionValueText;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] Button abilityButtonPrefab;
    [SerializeField] Slider staminaBar;
    [Header("Item Prefabs")]
    [SerializeField] GameObject itemImage;
    [SerializeField] GameObject itemButton;
    [Header("Transaction")]
    [SerializeField]public Transaction currentTransaction;
    [Header("Encounter Variables")]
    [SerializeField]private int haggleCost = 5;
    
    public int Stamina { get => stamina; set => stamina = value; }

    // Start is called before the first frame update
    void Start()
    {
        offerChanger = hagglePanel.GetComponent<OfferChanger>();
        InstantiateAbilityButtons();
        //Set up UI
        offerText.text = currentOffer.ToString();
        currentTransaction.Offer = currentOffer;
        transactionValueText.text = currentOffer.ToString();
        goldText.text = playerInventory.GetCurrency().ToString();
        stamina = playerProfile.GetStamina();
        staminaBar.maxValue = stamina;
        staminaBar.value = stamina;
        satisfactionBubble.SetSatisfactionLevel(0.0f);
        //Hide canvases
        offerChanger.Hide();
        abilityPanel.SetActive(false);
        hagglingCanvas.SetActive(false);
    }

    void OnEnable()
    {
        PlayIntro();
    }

    // Update is called once per frame
    void Update()
    {
        goldText.text = playerInventory.GetCurrency().ToString();
        transactionValueText.text = currentTransaction.GetValue().ToString();
        staminaBar.value = stamina;
        if (!playerTurn)
        {
            HandleCustomerTurn();
            satisfactionBubble.SetSatisfactionLevel(activeCustomerProfile.CalculateFavorability(currentTransaction));
        }
    }

    public void Haggle()
    {
        if (stamina - haggleCost >= 0)
        {
            hagglePanel.SetActive(true);
            stamina -= haggleCost; 
        }
        else
        {
            Debug.Log("Not Enough Stamina");
        }
    }

    public void ExitHagglingMenu()
    {
        offerText.text = currentOffer.ToString();
        EndTurn();
    }

    public void Sell()
    {
        if (activeCustomerProfile.ProcessTransaction(currentTransaction))
        {
            playerInventory.AddCurrency(currentOffer);
            activeCustomer.desiredItem.Shelf.changeStock(activeCustomer.desiredItem, -1);
            //TODO: Handle experience and gold animations
            HideCanvas();
            activeCustomer.isFinishedShopping = true;//TODO: Temp, maybe later tie this to a list that when exhausted they will head for the exit.
            if (activeCustomer.isFinishedShopping)
            {
                activeCustomer.GoToExit();
            }
        }
    }

    public void Deal()
    {
        isAbilitiesVisible = !isAbilitiesVisible;
        abilityPanel.SetActive(isAbilitiesVisible);
    }

    public void NoDeal()
    {
        foreach(Transform transform in offeredItemsCanvas.transform) { Destroy(transform.gameObject); }
        ReturnItems();
        HideCanvas();
    }

    public void ToggleCanvas()
    {
        isCanvasActive = !isCanvasActive;
        hagglingCanvas.SetActive(isCanvasActive);
    }

    public void HideCanvas()
    {
        currentTransaction.ClearTransaction();
        hagglingCanvas.SetActive(false);
        playerController.controlActive = true;
    }

    public void SetActiveCustomer(GameObject gameObject)
    {
        activeCustomer = gameObject.GetComponent<CustomerController>();
        activeCustomerProfile = activeCustomer.customerProfile;
    }

    public void ShowCanvas()
    {
        playerController.controlActive = false;
        //Get and set up the item for sale.
        itemForSale = activeCustomer.desiredItem;
        //currentOffer = itemForSale.CalculateItemPrice();//TODO: This is a problem area. Resets the offer amount.
        imageHolder.sprite = itemForSale.item.itemIcon;
        //Remove item for sale from player inventory and add it to transaction, moved to SetItemForSale, left here for reference
        //playerInventory.TakeItem(itemForSale);
        //currentTransaction.AddItem(itemForSale);
        //Set up UI
        //offerText.text = currentOffer.ToString();
        //goldText.text = playerInventory.GetCurrency().ToString();
        //stamina = playerProfile.GetStamina();
        //staminaBar.maxValue = stamina;
        //staminaBar.value = stamina;
        hagglingCanvas.SetActive(true);
    }

    public void SetItemForSale(ItemInstance item)
    {
        itemForSale = item;
        imageHolder.sprite = itemForSale.item.itemIcon;
        playerInventory.TakeItem(itemForSale);
        currentTransaction.AddItem(itemForSale);
        currentOffer = itemForSale.CalculateItemPrice();
        currentTransaction.Offer = currentOffer;
        offerText.text = currentOffer.ToString();
    }

    public void UseStamina(int usedStamina)
    {
        this.Stamina -= usedStamina;
        staminaBar.value = this.Stamina;
    }

    public void AddItemOffer(ItemInstance item)
    {
        GameObject newItemImage = Instantiate(itemImage, offeredItemsCanvas.transform);
        newItemImage.GetComponent<Image>().sprite = item.item.itemIcon;
        currentTransaction.AddItem(item);
        //satisfactionBubble.SetSatisfactionLevel((float)activeCustomerProfile.FavorabilityRating);
        playerInventory.TakeItem(item);
    }

    public void EndTurn()
    {
        satisfactionBubble.SetSatisfactionLevel(activeCustomerProfile.CalculateFavorability(currentTransaction));
        playerTurn = false;
    }

    private void InstantiateAbilityButtons()
    {
        foreach (Ability ability in playerProfile.Abilities)
        {
            Button newButton = Instantiate(abilityButtonPrefab, abilityPanel.transform);
            AbilityButton abilityButton = newButton.GetComponent<AbilityButton>();
            abilityButton.SetAbility(ability);

            newButton.onClick.AddListener(delegate { ability.Initialize(gameObject); });
            newButton.onClick.AddListener(delegate { UseStamina(ability.aAbilityCost); });
        }
    }

    private void HandleCustomerTurn()
    {
        currentOffer += activeCustomerProfile.HaggleTransaction(currentTransaction);
        currentTransaction.Offer = currentOffer;//Eh? not sure this will help
        offerText.text = currentOffer.ToString();
        playerTurn = true;
    }

    private void HandlePlayerTurn()
    {
        while (playerTurn)
        {
            //Maybe kinda needless, but I want this here as a placeholder for anything I want to do while it's the player's turn.
        }
    }

    private void ReturnItems()
    {
        foreach (ItemInstance item in currentTransaction.offeredItems)
        {
            playerInventory.GiveItem(item.item);
        }
    }

    private void PlayIntro()
    {
        //Play customer intro text
        introShown = true;
    }
}

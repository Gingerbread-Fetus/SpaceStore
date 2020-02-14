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

    public void ClearItems()
    {
        offeredItems.Clear();
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
    private ItemInstance itemForSale;
    [SerializeField]private CustomerController activeCustomer;//Serialized for debugging
    private CustomerProfile activeCustomerProfile;

    [Header("Game Session Properties")]
    [SerializeField] bool isCanvasActive;
    [SerializeField] float gameSpeed;
    [SerializeField] int stamina = 50;
    [SerializeField] bool playerTurn;
    [Header("Inventory references")]
    [SerializeField] StoreInventory playerInventory;
    [SerializeField] PlayerProfile playerProfile;
    [SerializeField] StoreInventory customerInventory;
    [Header("Haggling Canvas References")]
    [SerializeField] GameObject offeredItemsCanvas;
    [SerializeField] GameObject hagglingCanvas;
    [SerializeField] GameObject abilityPanel;
    [SerializeField] Image imageHolder;
    [SerializeField] TextMeshProUGUI offerText;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] Button abilityButtonPrefab;
    [SerializeField] Slider staminaBar;
    [Header("Item Prefabs")]
    [SerializeField] GameObject itemImage;
    [SerializeField] GameObject itemButton;
    [Header("Transaction")]
    [SerializeField]public Transaction currentTransaction;

    int currentOffer;

    public int Stamina { get => stamina; set => stamina = value; }

    // Start is called before the first frame update
    void Start()
    {
        InstantiateAbilityButtons();
        abilityPanel.SetActive(false);
        //hagglingCanvas.SetActive(isCanvasActive);//TODO: Make sure to uncomment this later
    }

    void OnEnable()
    {
        PlayIntro();
    }

    // Update is called once per frame
    void Update()
    {
        goldText.text = playerInventory.GetCurrency().ToString();
        if (!playerTurn)
        {
            HandleCustomerTurn();
        }
    }

    private void InstantiateAbilityButtons()
    {
        foreach(Ability ability in playerProfile.Abilities)
        {
            Button newButton = Instantiate(abilityButtonPrefab, abilityPanel.transform);
            AbilityButton abilityButton = newButton.GetComponent<AbilityButton>();
            abilityButton.SetAbility(ability);

            //TODO: Does this work?
            newButton.onClick.AddListener(delegate { ability.Initialize(gameObject); });
            newButton.onClick.AddListener(delegate { UseStamina(ability.aAbilityCost); });
        }
    }

    private void HandleCustomerTurn()
    {
        playerTurn = true;
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
            currentOffer += 10;
            currentTransaction.Offer = currentOffer;
            offerText.text = currentOffer.ToString();
            //The important part is that these methods end the turn
            playerTurn = false;
        }
    }

    public void Sell()
    {
        if (activeCustomerProfile.ProcessTransaction(currentTransaction))
        {
            playerInventory.AddCurrency(currentOffer);
            activeCustomer.desiredItem.Shelf.changeStock(activeCustomer.desiredItem, -1);
            //TODO: Handle experience and gold animations
            HideCanvas();
            activeCustomer.isFinishedShopping = true;//TODO: Temp, maybe later tie this to a list that when exhausted they willhead for the exit.
            if (activeCustomer.isFinishedShopping)
            {
                activeCustomer.GoToExit(); 
            }
        }
    }

    public void Deal()
    {
        //TODO: Handle deal selection and invoking.
        isAbilitiesVisible = !isAbilitiesVisible;
        abilityPanel.SetActive(isAbilitiesVisible);
    }

    public void NoDeal()
    {
        //TODO: Handle no deal stuff
        foreach(Transform transform in offeredItemsCanvas.transform) { Destroy(transform.gameObject); }
        ReturnItems();
        HideCanvas();
    }

    private void ReturnItems()
    {
        foreach(ItemInstance item in currentTransaction.offeredItems)
        {
            playerInventory.GiveItem(item.item);
        }
    }

    private void PlayIntro()
    {
        //Play customer intro text
        introShown = true;
    }

    public void ToggleCanvas()
    {
        isCanvasActive = !isCanvasActive;
        hagglingCanvas.SetActive(isCanvasActive);
    }

    public void HideCanvas()
    {
        currentTransaction.ClearItems();
        hagglingCanvas.SetActive(false);
    }

    public void SetActiveCustomer(GameObject gameObject)
    {
        activeCustomer = gameObject.GetComponent<CustomerController>();
        activeCustomerProfile = activeCustomer.customerProfile;
    }

    public void ShowCanvas()
    {
        //Get and set up the item for sale.
        itemForSale = activeCustomer.desiredItem;
        currentOffer = itemForSale.CalculateItemPrice();
        imageHolder.sprite = itemForSale.item.itemIcon;
        //Remove item for sale from player inventory and add it to transaction
        playerInventory.TakeItem(itemForSale);
        currentTransaction.AddItem(itemForSale);
        //Set up UI
        offerText.text = currentOffer.ToString();
        goldText.text = playerInventory.GetCurrency().ToString();
        stamina = playerProfile.GetStamina();
        staminaBar.maxValue = stamina;
        staminaBar.value = stamina;
        hagglingCanvas.SetActive(true);
    }

    public void SetItemForSale(ItemInstance item)
    {
        itemForSale = item;
        imageHolder.sprite = itemForSale.item.itemIcon;
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
        playerInventory.TakeItem(item);
    }
}

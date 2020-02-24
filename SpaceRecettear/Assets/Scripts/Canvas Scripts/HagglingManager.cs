using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// HagglingManager handles turns in the haggling screen. as well as transaction
/// data and the different canvases needed for handling the storefront scene
/// </summary>
public class HagglingManager : MonoBehaviour
{
    private bool introShown;
    private bool isAbilitiesVisible = false;
    private int moneyChange = 0;
    private int currentStamina;
    OfferChanger offerChanger;
    ClosingManager closingManager;
    CustomerManager customerManager;
    private ItemInstance itemForSale;
    [SerializeField]private CustomerController activeCustomer;//Serialized for debugging
    private CustomerProfile activeCustomerProfile;

    [Header("Game Session Properties")]
    [SerializeField] GameObject customerManagerObject;
    [SerializeField] PlayerController playerController;
    [SerializeField] bool isCanvasActive;
    [SerializeField] float gameSpeed;
    [SerializeField] int maxStamina = 50;
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
    [SerializeField] GameObject closingCanvas;
    [SerializeField] GameObject storeOverlay;
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
    
    public int Stamina { get => currentStamina; set => currentStamina = value; }

    // Start is called before the first frame update
    void Start()
    {
        currentTransaction = new Transaction();
        offerChanger = hagglePanel.GetComponent<OfferChanger>();
        closingManager = closingCanvas.GetComponent<ClosingManager>();
        customerManager = customerManagerObject.GetComponent<CustomerManager>();
        closingManager.StartingCash = playerInventory.GetCurrency();
        InstantiateAbilityButtons();
        //Set up UI
        offerText.text = currentOffer.ToString();
        transactionValueText.text = currentOffer.ToString();
        goldText.text = playerInventory.GetCurrency().ToString();
        maxStamina = playerProfile.GetStamina();
        currentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;
        satisfactionBubble.SetSatisfactionLevel(0.0f);
        //Hide canvases
        offerChanger.Hide();
        closingCanvas.SetActive(false);
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
        staminaBar.value = currentStamina;
        if (!playerTurn)
        {
            HandleCustomerTurn();
            satisfactionBubble.SetSatisfactionLevel(activeCustomerProfile.CalculateFavorability());
        }
        if (customerManager.WaveFinished)
        {
            ShowEndOfShift();
        }

    }

    public void Haggle()
    {
        if (currentStamina - haggleCost >= 0)
        {
            hagglePanel.SetActive(true);
            currentStamina -= haggleCost; 
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
        if (activeCustomerProfile.ProcessTransaction())
        {
            playerInventory.AddCurrency(currentOffer);
            moneyChange += currentOffer;
            closingManager.TotalSales = moneyChange;
            activeCustomer.desiredItem.Shelf.changeStock(activeCustomer.desiredItem, -1);
            //TODO: Handle experience and gold animations
            currentTransaction.ClearTransaction();
            HideCanvas();
            activeCustomer.isFinishedShopping = true;//TODO: Temp, maybe later tie this to a list that when exhausted they will head for the exit.
            if (activeCustomer.isFinishedShopping)
            {
                activeCustomer.GoToExit();
            }
            currentStamina = maxStamina;
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
        //currentOffer = itemForSale.CalculateItemPrice();//This resets the offer amount.
        imageHolder.sprite = itemForSale.item.itemIcon;
        activeCustomerProfile.SetTransaction(currentTransaction);
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
        satisfactionBubble.SetSatisfactionLevel(activeCustomerProfile.CalculateFavorability());
        playerTurn = false;
    }

    public void ShowEndOfShift()
    {
        //Make sure to hide other canvases
        storeOverlay.gameObject.SetActive(false);
        closingCanvas.SetActive(true);
        closingManager.EndingCash = playerInventory.GetCurrency();
        closingManager.EndOfSession();
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
        currentTransaction.Offer = currentOffer;
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

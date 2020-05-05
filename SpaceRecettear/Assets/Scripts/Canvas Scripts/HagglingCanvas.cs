using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HagglingCanvas : MonoBehaviour
{
    int currentStamina;
    OfferChanger offerChanger;
    CustomerManager customerManager;
    [SerializeField] private CustomerController activeCustomer;//Serialized for debugging
    private CustomerProfile activeCustomerProfile;
    private int moneyChange = 0;
    private bool isAbilitiesVisible = false;

    [HideInInspector] public int currentOffer;
    private bool introShown;
    private bool isPlayerTurn = true;

    public int Stamina { get => currentStamina; set => currentStamina = value; }

    [Header("Haggling Canvas References")]
    [SerializeField] GameObject offeredItemsCanvas;
    [SerializeField] GameObject hagglePanel;
    [SerializeField] GameObject abilityPanel;
    [SerializeField] Slider staminaBar;
    [SerializeField] TextMeshProUGUI offerText;
    [SerializeField] TextMeshProUGUI transactionValueText;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] GameObject closingCanvas;
    [SerializeField] Image imageHolder;
    [SerializeField] Button abilityButtonPrefab;
    [SerializeField] ClosingManager closingManager;
    [Header("Item References")]
    [SerializeField] GameObject itemImage;
    [SerializeField] GameObject itemButton;
    [Header("Properties")]
    [SerializeField] int maxStamina = 50;// Todo, rather than serialize this, make it part of the player profile
    [SerializeField] int haggleCost = 5;// Todo, rather than serialize this, make it part of the player profile
    [SerializeField] SatisfactionBubble satisfactionBubble;
    [SerializeField] PlayerProfile playerProfile;
    [SerializeField] StoreInventory playerInventory;
    [Header("Transaction")]
    [SerializeField] public Transaction currentTransaction;//I think I'm serializing this for debugging
    private ItemInstance itemForSale;

    void Start()
    {
        //Hide all canvases
        offerChanger.ResolveAndHide();
        closingCanvas.SetActive(false);
        abilityPanel.SetActive(false);
        gameObject.SetActive(false);
    }

    public void AddToOfferedItems(ItemInstance selectedItem)
    {
        currentTransaction.AddItem(selectedItem);
    }

    // Update is called once per frame
    void Update()
    {
        goldText.text = playerInventory.GetCurrency().ToString();
        transactionValueText.text = currentTransaction.GetValue().ToString();
        staminaBar.value = currentStamina;
        if (!isPlayerTurn)
        {
            HandleCustomerTurn();
            satisfactionBubble.SetSatisfactionLevel(activeCustomerProfile.CalculateFavorability());
        }
    }

    void OnEnable()
    {
        PlayIntro();
        currentTransaction = new Transaction();
        offerChanger = hagglePanel.GetComponent<OfferChanger>();
        customerManager = FindObjectOfType<CustomerManager>();
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
    }

    private void HandleCustomerTurn()
    {
        print("Handling customer turn stuff here");
        isPlayerTurn = true;
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

    public void UseStamina(int usedStamina)
    {
        this.Stamina -= usedStamina;
        staminaBar.value = this.Stamina;
    }

    public void ShowCanvas()
    {
        gameObject.SetActive(true);
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

    public void HideCanvas()
    {
        gameObject.SetActive(false);
        FindObjectOfType<PlayerController>().controlActive = true;
        //BroadcastMessage("EnableMovement");//Todo implement something like this instead.
    }

    public void Deal()
    {
        isAbilitiesVisible = !isAbilitiesVisible;
        abilityPanel.SetActive(isAbilitiesVisible);
    }

    public void NoDeal()
    {
        foreach (Transform transform in offeredItemsCanvas.transform) { Destroy(transform.gameObject); }
        ReturnItems();
        HideCanvas();
    }

    public void Haggle()
    {
        if (currentStamina - haggleCost >= 0)
        {
            offerChanger.gameObject.SetActive(true);
            currentStamina -= haggleCost;
        }
        else
        {
            Debug.Log("Not Enough Stamina");
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

    public void SetActiveCustomer(CustomerController newCustomer)
    {
        activeCustomer = newCustomer;
        activeCustomerProfile = activeCustomer.customerProfile;
    }

    public void EndTurn()
    {
        satisfactionBubble.SetSatisfactionLevel(activeCustomerProfile.CalculateFavorability());
        isPlayerTurn = false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HagglingManager handles turns in the haggling screen.
/// </summary>
public class HagglingManager : MonoBehaviour
{
    private bool introShown;
    private bool isAbilitiesVisible = false;
    private ItemInstance itemForSale;

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

    int currentOffer;

    public int Stamina { get => stamina; set => stamina = value; }

    // Start is called before the first frame update
    void Start()
    {
        InstantiateAbilityButtons();
        abilityPanel.SetActive(false);
        //hagglingCanvas.SetActive(isCanvasActive);//TODO: Make sure to uncomment this later
        itemForSale = playerInventory.RandomItem();
        currentOffer = itemForSale.CalculateItemPrice();
        imageHolder.sprite = itemForSale.item.itemIcon;
        offerText.text = currentOffer.ToString();
        goldText.text = playerInventory.GetCurrency().ToString();
        stamina = playerProfile.GetStamina();
        staminaBar.maxValue = stamina;
        staminaBar.value = stamina;
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
        currentOffer += 10;
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

    public void Haggle()
    {
        if (playerTurn)
        {
            //The important part is that these methods end the turn
            playerTurn = false;
        }
    }

    public void Sell()
    {
        playerInventory.SellItem(itemForSale, currentOffer);
        //TODO: Handle experience and gold animations
        HideCanvas();
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
        HideCanvas();
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
        hagglingCanvas.SetActive(false);
    }

    public void ShowCanvas()
    {
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
        playerInventory.TakeItem(item);
    }
}

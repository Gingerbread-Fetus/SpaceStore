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
    private ItemInstance itemForSale;

    [SerializeField] bool isCanvasActive;
    [SerializeField] float gameSpeed;
    [SerializeField] bool playerTurn;
    [SerializeField] StoreInventory playerInventory;
    [SerializeField] PlayerProfile playerProfile;
    [SerializeField] StoreInventory customerInventory;
    [SerializeField] Image imageHolder;
    [SerializeField] TextMeshProUGUI offerText;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] GameObject hagglingCanvas;

    int currentOffer;
    // Start is called before the first frame update
    void Start()
    {
        hagglingCanvas.SetActive(isCanvasActive);
        itemForSale = playerInventory.RandomItem();
        currentOffer = itemForSale.CalculateItemPrice();
        imageHolder.sprite = itemForSale.item.itemIcon;
        offerText.text = currentOffer.ToString();
        goldText.text = playerInventory.GetCurrency().ToString();
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

    public void SetItemForSale(ItemInstance item)
    {
        itemForSale = item;
        imageHolder.sprite = itemForSale.item.itemIcon;
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
        HideCanvas();
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
}

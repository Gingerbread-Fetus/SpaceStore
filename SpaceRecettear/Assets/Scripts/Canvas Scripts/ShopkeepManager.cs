using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopkeepManager : MonoBehaviour
{
    [SerializeField] StoreInventory storeInventoryObject;
    [SerializeField] GameObject storeInventoryPanel;
    [SerializeField] GameObject transactionPanel;
    [SerializeField] ShopTransactionPrompt promptPanel;
    [SerializeField] Button NPCTransactionButtonPrefab;
    [SerializeField] TextMeshProUGUI priceTextObject;

    List<ItemInstance> storeInventoryList;
    List<GuildShopTransactionButton> transactionList;
    int totalItemCost = 0;
    bool transactionChanged = false;

    // Start is called before the first frame update
    void Start()
    {
        promptPanel.gameObject.SetActive(false);
        foreach (Transform child in storeInventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in transactionPanel.transform)
        {
            Destroy(child.gameObject);
        }
        transactionList = new List<GuildShopTransactionButton>();
        storeInventoryList = storeInventoryObject.GetInventory();
        PopulateInventory();
    }

    // Update is called once per frame
    void Update()
    {
        if (transactionChanged)
        {
            
        }
    }

    private void PopulateInventory()
    {
        foreach(ItemInstance item in storeInventoryList)
        {
            AddNewTransactionButton(item, storeInventoryPanel);
        }
    }

    public void MoveItemToTransaction(GuildShopTransactionButton desiredItem, int amountToAdd)
    {
        int indexOf = transactionList.IndexOf(desiredItem);
        if (indexOf >= 0)
        {
            transactionList[indexOf].heldItem.stock += amountToAdd;
        }
        else
        {
            GuildShopTransactionButton newTransactionButton = AddNewTransactionButton(desiredItem.heldItem, transactionPanel);
            newTransactionButton.heldItem = new ItemInstance(desiredItem.heldItem);
            newTransactionButton.heldItem.stock = amountToAdd;
            transactionList.Add(newTransactionButton);
        }
        transactionChanged = !transactionChanged;
    }

    public void RemoveItemFromTransaction(GuildShopTransactionButton itemToRemove, int amountToRemove)
    {
        int indexOf = transactionList.IndexOf(itemToRemove);
        if (indexOf >= 0)
        {
            int existingStock = transactionList[indexOf].heldItem.stock;
            if(existingStock > amountToRemove)
            {
                transactionList[indexOf].heldItem.stock -= amountToRemove;
            }
            else
            {
                transactionList.RemoveAt(indexOf);
                Destroy(itemToRemove.gameObject);
            }
        }
        transactionChanged = !transactionChanged;
    }

    private GuildShopTransactionButton AddNewTransactionButton(ItemInstance item, GameObject newPanelParent)
    {
        Button newButton = Instantiate(NPCTransactionButtonPrefab);
        newButton.transform.SetParent(newPanelParent.transform, false);
        Image image = newButton.GetComponent<Image>();
        image.sprite = item.item.itemIcon;

        GuildShopTransactionButton transactionButton = newButton.GetComponent<GuildShopTransactionButton>();
        transactionButton.heldItem = item;

        newButton.onClick.AddListener(delegate { ShowPrompt(transactionButton); });

        return transactionButton;
    }

    private void ShowPrompt(GuildShopTransactionButton buttonClicked)
    {
        promptPanel.gameObject.SetActive(true);
        promptPanel.setDesiredItem(buttonClicked);

        if (buttonClicked.transform.IsChildOf(storeInventoryPanel.transform))
        {
            promptPanel.transactionState = ShopTransactionPrompt.TransactionType.TakeFromShop;
        }
        else
        {
            promptPanel.transactionState = ShopTransactionPrompt.TransactionType.GiveToShop;
        }
    }

}

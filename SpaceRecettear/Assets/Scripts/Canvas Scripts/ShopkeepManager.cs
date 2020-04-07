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
    Transaction shopTransaction;

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
        shopTransaction = new Transaction();
        storeInventoryList = storeInventoryObject.GetInventory();
        PopulateInventory();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void PopulateInventory()
    {
        foreach(ItemInstance item in storeInventoryList)
        {
            AddNewItemButton(item, storeInventoryPanel);
            return;
        }
    }

    public void MoveItemToTransaction(ItemInstance desiredItem)
    {
        shopTransaction.AddItem(desiredItem);
    }

    private ItemButton AddNewItemButton(ItemInstance item, GameObject newPanelParent)
    {
        Button newButton = Instantiate(NPCTransactionButtonPrefab);
        newButton.transform.SetParent(newPanelParent.transform, false);
        Image image = newButton.GetComponent<Image>();
        image.sprite = item.item.itemIcon;

        ItemButton itemButton = newButton.GetComponent<ItemButton>();
        itemButton.heldItem = item;

        newButton.onClick.AddListener(delegate { ShowPrompt(itemButton); });

        return itemButton;
    }

    private void ShowPrompt(ItemButton buttonClicked)
    {
        promptPanel.gameObject.SetActive(true);
        promptPanel.setDesiredItem(buttonClicked.heldItem);
    }

}

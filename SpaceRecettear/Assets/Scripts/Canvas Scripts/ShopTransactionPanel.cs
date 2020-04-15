using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTransactionPanel : MonoBehaviour
{
    [SerializeField] Transform parent;//Holder for the object new buttons get parented to
    [SerializeField] NPCTransactionButton buttonPrefab;
    [SerializeField] StoreInventory storeInventoryObject;

    List<ItemInstance> availableItems;

    // Start is called before the first frame update
    void Start()
    {
        availableItems = storeInventoryObject.GetInventory();
        foreach(ItemInstance item in availableItems)
        {
            AddNewItemButton(item);
        }
    }

    public void AddNewItemButton(ItemInstance item)
    {
        NPCTransactionButton newButton = Instantiate(buttonPrefab, parent);
        newButton.HeldItem = item;
    }

    public void ReturnItem(ItemInstance item, int howMany)
    {
        storeInventoryObject.GiveItem(item, howMany);
    }
}

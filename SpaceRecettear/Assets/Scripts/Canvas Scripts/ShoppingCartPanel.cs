using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShoppingCartPanel : MonoBehaviour
{
    [SerializeField] Transform parent;//Holder for the object new buttons get parented to
    [SerializeField] ShoppingCartTransactionButton buttonPrefab;
    [SerializeField] StoreInventory playerInventoryObject;
    [SerializeField] TextMeshProUGUI priceText;

    ShopTransactionPanel shopPanel;

    List<ItemInstance> shoppingCart = new List<ItemInstance>();
    int costOfTransaction = 0;

    void Start()
    {
        priceText.text = "0";
        shopPanel = FindObjectOfType<ShopTransactionPanel>();
    }

    void Update()
    {
        priceText.text = costOfTransaction.ToString();
    }

    public void AddItemToShoppingCart(ItemInstance newItem)
    {
        int indexOf = shoppingCart.IndexOf(newItem);
        if(indexOf >= 0)
        {
            shoppingCart[indexOf].stock += newItem.stock;
        }
        else
        {
            ItemInstance newItemInstance = new ItemInstance(newItem);
            ShoppingCartTransactionButton newButton = Instantiate(buttonPrefab, parent);
            newButton.HeldItem = newItemInstance;
            shoppingCart.Add(newItemInstance);
        }
        costOfTransaction += newItem.stock * newItem.item.baseSellPrice;
    }

    public void RemoveFromShoppingCart(ShoppingCartTransactionButton itemToRemove, int howMany)
    {
        int indexOf = shoppingCart.IndexOf(itemToRemove.HeldItem);
        if (indexOf >= 0)
        {
            if(itemToRemove.HeldItem.stock - howMany <= 0)
            {
                howMany = itemToRemove.HeldItem.stock;
                costOfTransaction -= howMany * itemToRemove.HeldItem.item.baseSellPrice;
                shopPanel.ReturnItem(itemToRemove.HeldItem, howMany);
                itemToRemove.HeldItem.stock = 0;
                shoppingCart.RemoveAt(indexOf);
            }
            else
            {
                costOfTransaction -= howMany * itemToRemove.HeldItem.item.baseSellPrice;
                shopPanel.ReturnItem(itemToRemove.HeldItem, howMany);
                itemToRemove.HeldItem.stock -= howMany;
            }
        }
    }

    public void FinishTransaction()
    {
        if (playerInventoryObject.GetCurrency() > costOfTransaction)
        {
            foreach (ItemInstance item in shoppingCart)
            {
                playerInventoryObject.GiveItem(item);
            }
            playerInventoryObject.AddCurrency(-costOfTransaction);
            ClearTransaction();
        }
        else
        {
            print("Not enough cash!");
        }
    }

    private void ClearTransaction()
    {
        costOfTransaction = 0;
        shoppingCart = new List<ItemInstance>();
        foreach(Transform item in parent)
        {
            Destroy(item.gameObject);
        }
    }
}

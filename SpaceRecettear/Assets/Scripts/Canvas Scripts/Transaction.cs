using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Transaction
{
    [SerializeField] public List<ItemInstance> offeredItems;
    int totalValue;
    int offer;

    public int Offer { get => offer; set => offer = value; }

    public Transaction()
    {
        totalValue = 0;
        offer = 0;
        offeredItems = new List<ItemInstance>();
    }

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

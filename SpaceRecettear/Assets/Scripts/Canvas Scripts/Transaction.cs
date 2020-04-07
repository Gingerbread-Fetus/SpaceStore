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
        totalValue = totalValue += newItem.item.baseSellPrice;

        int indexOf = offeredItems.IndexOf(newItem);
        if(indexOf > -1)
        {
            offeredItems[indexOf].stock += newItem.stock;
        }
        else
        {
            offeredItems.Add(newItem);
        }
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

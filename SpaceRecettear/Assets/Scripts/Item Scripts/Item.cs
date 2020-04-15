﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item : ScriptableObject
{   
    [SerializeField] string itemName;
    [SerializeField] public Sprite itemIcon;
    [SerializeField] public int baseSellPrice;
    public string ItemName { get => itemName;}

    public override bool Equals(System.Object obj)
    {
        if (obj == null || obj.GetType() != this.GetType()) {return false;}
        else
        {
            Item c = obj as Item;
            return this.itemName.Equals(c.itemName);
        }
    }

    public override int GetHashCode()
    {
        var hashCode = -828962279;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(itemName);
        return hashCode;
    }
}

/// <summary>
/// A class that holds a real instance of a ScriptableObject item.
/// Allows us to have copies with mutable data.
/// The fields on this class are shared attributes that all items should have.
/// </summary>
[System.Serializable]
public class ItemInstance
{
    // Reference to scriptable object "template".
    public Item item;
    int sellPrice;
    public bool isLimited = false;
    // Object-specific data.
    [SerializeField] bool boomItem = false;
    [SerializeField] public int stock = 1;
    [SerializeField, Range(0, 10)] public int quality = 0;
    [SerializeField] private Shelf shelf = null;
    public Shelf Shelf { get => shelf; set => shelf = value; }//Serializing for debugging purposes

    public ItemInstance() { }//Actually don't know what I may need to do with this.

    public ItemInstance(Item item)
    {
        shelf = null;
        this.item = item;
        quality = 0;
    }

    public ItemInstance(Item item, int itemQuality)
    {
        this.item = item;
        this.quality = itemQuality;
    }

    public ItemInstance(ItemInstance itemInstance)
    {
        this.item = itemInstance.item;
        this.boomItem = itemInstance.boomItem;
        this.stock = itemInstance.stock;
        this.quality = itemInstance.quality;
        this.sellPrice = CalculateItemPrice();
    }


    public override bool Equals(object obj)
    {
        if(obj == null || this.GetType() != obj.GetType()) { return false; }
        ItemInstance c = obj as ItemInstance;
        return this.item.name.Equals(c.item.name) && c.quality == this.quality;
    }

    public override int GetHashCode()
    {
        var hashCode = 1030684156;
        hashCode = hashCode * -1521134295 + EqualityComparer<Item>.Default.GetHashCode(item);
        hashCode = hashCode * -1521134295 + quality.GetHashCode();
        return hashCode;
    }

    /// <summary>
    /// Item price is based off of the base price and the quality of the item.
    /// </summary>
    /// <returns></returns>
    public int CalculateItemPrice()
    {
        return item.baseSellPrice + (10 * quality);
    }

    /// <summary>
    /// This method just checks that items are of the same type by their name.
    /// </summary>
    /// <param name="otherItem"></param>
    /// <returns></returns>
    public bool ItemEqualsByType(ItemInstance otherItem)
    {
        return this.item.name.Equals(otherItem.item.name);
    }
}

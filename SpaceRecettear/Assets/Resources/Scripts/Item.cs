using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item : ScriptableObject
{   
    [SerializeField] string itemName;
    [SerializeField] Sprite itemIcon;
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
    // Object-specific data.
    [SerializeField] bool boomItem;
    [SerializeField] int baseSellPrice;
    [SerializeField] public int stock = 1;
    [SerializeField, Range(0, 10)] public int quality;

    public ItemInstance(Item item)
    {
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
        this.baseSellPrice = itemInstance.baseSellPrice;
        this.stock = itemInstance.stock;//TODO: This doesn't seem quite right, make sure that stock is changed and set correctly
        this.quality = itemInstance.quality;
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item : ScriptableObject
{   
    [SerializeField] string itemName;
    [SerializeField] Sprite itemIcon;
    [SerializeField] bool boomItem;
    [SerializeField] int baseSellPrice;
    [SerializeField] int stock;
    [SerializeField, Range(0, 10)] int quality;
    public string ItemName { get => itemName;}
    public int Quality { get => quality;}
}

// A class that holds a real instance of a ScriptableObject item.
// Allows us to have copies with mutable data.
[System.Serializable]
public class ItemInstance
{
    // Reference to scriptable object "template".
    public Item item;
    // Object-specific data.
    [Range(0, 10)]
    public int quality;
    public int stock;

    public ItemInstance(Item item)
    {
        this.item = item;
        this.quality = item.Quality;
    }
}

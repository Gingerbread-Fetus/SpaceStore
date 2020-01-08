using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the player's store inventory across all screens in the game. Should not ever be destroyed
/// </summary>
[CreateAssetMenu(menuName = "Items/Inventory", fileName = "Inventory.asset")]
[System.Serializable]
public class StoreInventory : ScriptableObject
{

    //Config Params
    //[SerializeField] MyDictionary inventory;
    [SerializeField] List<ItemInstance> inventory;
    //[SerializeField] List<ItemInstance> itemList;
    [SerializeField] int currency = 0;

    ///A list of items that are currently in the inventory. Left visible to set starting values and for debugging

    private static StoreInventory _instance;
    public static StoreInventory Instance
    {
        get
        {
            if (!_instance)
            {
                StoreInventory[] tmp = Resources.FindObjectsOfTypeAll<StoreInventory>();
                if (tmp.Length > 0)
                {
                    _instance = tmp[0];
                    Debug.Log("Found inventory as: " + _instance);
                }
                else
                {
                    Debug.Log("Did not find inventory, loading from file or template.");
                    SaveManager.LoadOrInitializeInventory();
                }
            }

            return _instance;
        }
    }

    public int GetCurrency()
    {
        return currency;
    }

    public List<ItemInstance> GetInventory()
    {
        return inventory;
    }

    public static void LoadFromJSON(string path)
    {
        if (_instance) DestroyImmediate(_instance);
        _instance = ScriptableObject.CreateInstance<StoreInventory>();
        JsonUtility.FromJsonOverwrite(System.IO.File.ReadAllText(path), _instance);
        _instance.hideFlags = HideFlags.HideAndDontSave;
    }

    public void SaveToJSON(string path)
    {
        Debug.LogFormat("Saving inventory to {0}", path);
        System.IO.File.WriteAllText(path, JsonUtility.ToJson(this, true));
    }

    public static void InitializeFromDefault()
    {
        if (_instance) DestroyImmediate(_instance);
        _instance = Instantiate((StoreInventory)Resources.Load("InventoryTemplate"));
        _instance.hideFlags = HideFlags.HideAndDontSave;
    }

    /* Start of Inventory Methods */
        
    /// <summary>
    /// Adds an item to the inventory. Functional command for purchasing something with this inventory. Decrements currency.
    /// TODO: NPCs should have infinite money, but for now I've just given them 2 billion instead.
    /// </summary>
    /// <param name="newItem">The instance of the item to add to the dictionary</param>
    /// <param name="count">The number of this item to add</param>
    public void GiveItem(Item addItem)
    {
        ItemInstance newItem = new ItemInstance(addItem);
        Debug.Log(newItem);
        if (inventory.Contains(newItem))//if it's already in the inventory add number asked for.
        {
            int i = inventory.IndexOf(newItem);
            inventory[i].stock += 1;
            Debug.Log(newItem.item.name + " of quality: " + inventory[i].quality + " has new stock: " + inventory[i].stock);
        }
        else
        {
            Debug.Log("New item added.");
            inventory.Add(newItem);
        }
    }

    public void GiveItem(Item addItem, int Quality)
    {
        ItemInstance newItem = new ItemInstance(addItem);
        if (inventory.Contains(newItem))//if it's already in the inventory add number asked for.
        {
            int i = inventory.IndexOf(newItem);
            inventory[i].stock += 1;
            Debug.Log(newItem.item.name + " of quality: " + inventory[i].quality + " has new stock: " + inventory[i].stock);
        }
        else
        {
            Debug.Log("New item added.");
            inventory.Add(newItem);
        }
    }

    /// <summary>
    /// Returns a random item that exists in the inventory.
    /// </summary>
    /// <returns></returns>
    public ItemInstance RandomItem()
    {
        int index = (int)UnityEngine.Random.Range(0, inventory.Count);
        return inventory[index];
    }

    /// <summary>
    /// Used for when an item is taken from the inventory. Does nothing and returns false if the item isn't in the inventory.
    /// </summary>
    /// <param name="itemToTake"></param>
    public bool TakeItem(Item itemToTake)
    {
        ItemInstance soldItem = new ItemInstance(itemToTake);
        if (inventory.Contains(soldItem))
        {
            int i = inventory.IndexOf(soldItem);

            if (inventory[i].stock > 1)
            {
                inventory[i].stock -= 1;
                Debug.Log("Stock of " + soldItem.item.name + " was reduced");
            }
            else
            {
                inventory.Remove(soldItem);
                Debug.Log("Stock of " + soldItem.item.name + " reached zero");
            }
            return true;
        }
        Debug.Log("This item was not found in the inventory");
        return false;
    }

    public bool TakeItem(ItemInstance itemToTake)
    {
        if (inventory.Contains(itemToTake))
        {
            int i = inventory.IndexOf(itemToTake);

            if (inventory[i].stock > 1)
            {
                inventory[i].stock -= 1;
                Debug.Log("Stock of " + itemToTake.item.name + " was reduced");
            }
            else
            {
                inventory.Remove(itemToTake);
                Debug.Log("Stock of " + itemToTake.item.name + " reached zero");
            }
            return true;
        }
        Debug.Log("This item was not found in the inventory");
        return false;
    }

    public void SellItem(ItemInstance itemToSell, int itemPrice)
    {
        if (TakeItem(itemToSell))
        {
            Debug.Log("Item being sold for: " + itemToSell.CalculateItemPrice());
            currency += itemPrice;
        }
    }

    private void Save()
    {
        SaveManager.SaveInventory();
    }
}

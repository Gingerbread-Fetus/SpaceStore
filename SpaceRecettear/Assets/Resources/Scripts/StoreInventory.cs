using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the player's store inventory across all screens in the game. Should not ever be destroyed
/// </summary>
[CreateAssetMenu(menuName = "Items/Inventory", fileName = "Inventory.asset")]
[System.Serializable]
public class StoreInventory : ScriptableObject
{
    
    Dictionary<string, Item> inventory;
    [SerializeField] List<ItemInstance> itemList;
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

    public void Start()
    {
        //Take every object in the ItemList and add it to the dictionary
        foreach (ItemInstance itemInstance in itemList)
        {
            inventory.Add(itemInstance.item.name, itemInstance.item);
        }
    }

    //Add an item to the inventory
    public void AddItem(Item newItem)
    {
        if (inventory[newItem.ItemName])//if it's already in the inventory add number asked for.
        {
           
        }
        else
        {
            inventory.Add(newItem.name, newItem);
        }
    }

    public void SellItem(Item itemToSell)
    {
        //TODO: Implement this method
    }

    private void Save()
    {
        SaveManager.SaveInventory();
    }
}

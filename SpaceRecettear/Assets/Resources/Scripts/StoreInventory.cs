using System;
using System.Collections;
using RotaryHeart.Lib;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

/// <summary>
/// This is the player's store inventory across all screens in the game. Should not ever be destroyed
/// </summary>
[CreateAssetMenu(menuName = "Items/Inventory", fileName = "Inventory.asset")]
[System.Serializable]
public class StoreInventory : ScriptableObject
{

    //Config Params
    [SerializeField] MyDictionary inventory;
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

    public void Awake()
    {
        //Take every object in the ItemList and add it to the dictionary
        //foreach (ItemInstance itemInstance in itemList)
        //{
        //    if (itemInstance != null)
        //    {
        //        inventory.Add(itemInstance.GetHashCode(), itemInstance); 
        //    }
        //}
    }

    /// <summary>
    /// Add an item to the inventory.
    /// </summary>
    /// <param name="newItem">The instance of the item to add to the dictionary</param>
    /// <param name="count">The number of this item to add</param>
    public void AddItem(Item addItem)
    {
        ItemInstance newItem = new ItemInstance(addItem);
        Debug.Log(newItem);
        if (inventory.ContainsKey(newItem.GetHashCode()))//if it's already in the inventory add number asked for.
        {
            inventory[newItem.GetHashCode()].stock += 1;
            Debug.Log("Added one apple to existing stock.");
        }
        else
        {
            Debug.Log("New item added.");
            inventory.Add(newItem.GetHashCode(), newItem);
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
/// <summary>
/// Declaration of the dictionary used for the inventory. Uses an int as a key for some wonkiness in the editor view if you
/// just use another iteminstance.
/// </summary>
[Serializable]
public class MyDictionary : SerializableDictionaryBase<int, ItemInstance> { }

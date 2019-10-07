using System;
using System.IO;
using UnityEngine;

public class SaveManager
{
    public static void LoadOrInitializeInventory()
    {
        // Saving and loading.
        if (File.Exists(Path.Combine(Application.persistentDataPath, "inventory.json")))
        {
            Debug.Log("Found file inventory.json, loading inventory.");
            StoreInventory.LoadFromJSON(Path.Combine(
                Application.persistentDataPath, "inventory.json"));
        }
        else
        {
            Debug.Log("Couldn't find inventory.json, loading from template.");
            StoreInventory.InitializeFromDefault();
        }
    }

    public static void SaveInventory()
    {
        StoreInventory.Instance.SaveToJSON(Path.Combine(
            Application.persistentDataPath, "inventory.json"));
    }


    // Load from the default, for situations where we just want to reset.
    public static void LoadFromTemplate()
    {
        StoreInventory.InitializeFromDefault();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    [SerializeField] StoreInventory storeInventory;
    //During "battle" scenes, this is the NPC inventory, but during shopping scenes this is the players inventory.
    [SerializeField] StoreInventory customerInventory;
    //TODO: Implement movement between inventories
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Buys item from customer for the store
    /// </summary>
    public void BuyItem(Item item)
    {
        if (customerInventory.TakeItem(item))
        {
            storeInventory.GiveItem(item);
        }
    }

    /// <summary>
    /// Sells item from store to customer.
    /// </summary>
    public void SellItem(Item item)
    {
        if (storeInventory.TakeItem(item))
        {
            customerInventory.GiveItem(item);
        }
    }
}

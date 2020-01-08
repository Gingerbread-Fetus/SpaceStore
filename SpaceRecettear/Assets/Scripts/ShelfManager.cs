using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShelfManager : MonoBehaviour
{
    private bool isShowing;
    [SerializeField] StoreInventory playerInventory;
    [SerializeField] GameObject playerInventoryPanel;
    [SerializeField] GameObject shelfInventoryPanel;
    [SerializeField] Button itemButtonPrefab;

    ItemInstance shelvedItem;
    // Start is called before the first frame update
    void Start()
    {
        //For every item in the player inventory, populate the inventory panel with an item button
        List<ItemInstance> items = playerInventory.GetInventory();
        foreach(ItemInstance item in items)
        {
            Button newButton = Instantiate(itemButtonPrefab);
            newButton.transform.SetParent(playerInventoryPanel.transform, false);
            Image image = newButton.GetComponent<Image>();
            image.sprite = item.item.itemIcon;
            newButton.GetComponent<ItemButton>().heldItem = item;
            newButton.onClick.AddListener(delegate { MoveItem(newButton); });
        }

        isShowing = false;
        gameObject.SetActive(false);
    }

    //Moves item from inventory to shelf
    private void MoveItem(Button newButton)
    {
        if (newButton.transform.IsChildOf(playerInventoryPanel.transform))
        {
            newButton.transform.SetParent(shelfInventoryPanel.transform, false); 
        }
        else
        {
            newButton.transform.SetParent(playerInventoryPanel.transform, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// This method is called when a shelf is interacted with. It displays on this
    /// UI what item, if any, is on that shelf.
    /// </summary>
    /// <param name="displayItem"></param>
    public void ChangeDisplayItem(ItemInstance displayItem)
    {
        if (displayItem != null)
        {
            Button newButton = Instantiate(itemButtonPrefab);
            Image image = newButton.GetComponent<Image>();
            image.sprite = displayItem.item.itemIcon;
            newButton.transform.SetParent(shelfInventoryPanel.transform, false); 
        }
    }

    public void HideOrShow()
    {
        isShowing = !isShowing;
        gameObject.SetActive(isShowing);

        foreach(ItemButton item in shelfInventoryPanel.GetComponentsInChildren<ItemButton>())
        {
            Destroy(item);
        }
    }
}

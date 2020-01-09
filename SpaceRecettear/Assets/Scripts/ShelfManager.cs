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
    Shelves activeShelf = null;
    List<ItemInstance> items;
    private bool isModified = false;

    // Start is called before the first frame update
    void Start()
    {
        //For every item in the player inventory, populate the inventory panel with an item button
        items = playerInventory.GetInventory();
        foreach(ItemInstance item in items)
        {
            AddNewItemButton(item, playerInventoryPanel);
        }

        isShowing = false;
        gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (isActiveAndEnabled && isModified)
        {
            foreach (ItemButton itemButton in playerInventoryPanel.transform.GetComponentsInChildren<ItemButton>())
            {
                itemButton.stockNumberText.text = playerInventory.GetStock(itemButton.heldItem).ToString();
            }

            if (activeShelf)
            {
                ItemButton[] itemButtonsArray = shelfInventoryPanel.transform.GetComponentsInChildren<ItemButton>();
                List<ItemButton> itemButtons = new List<ItemButton>(itemButtonsArray);
                foreach (ItemButton itemButton in itemButtons)
                {
                    if (activeShelf.heldItems.Contains(itemButton))
                    {
                        int itemIndex = activeShelf.heldItems.IndexOf(itemButton);
                        int itemStock = activeShelf.heldItems[itemIndex].heldItem.stock;
                        itemButton.stockNumberText.text = itemStock.ToString(); 
                    }
                }
                foreach(ItemButton itemButton in activeShelf.heldItems)
                {
                    if (!itemButtons.Contains(itemButton))
                    {
                        //create and add a button for it
                        AddNewItemButton(itemButton.heldItem, shelfInventoryPanel);
                    }
                }
            } 
        }
        isModified = false;
    }

    /// <summary>
    /// Creates a new ItemButton, and adds it to the provided game object as a child.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="newPanelParent"></param>
    private ItemButton AddNewItemButton(ItemInstance item, GameObject newPanelParent)
    {
        Button newButton = Instantiate(itemButtonPrefab);
        newButton.transform.SetParent(newPanelParent.transform, false);
        Image image = newButton.GetComponent<Image>();
        image.sprite = item.item.itemIcon;

        ItemButton itemButton = newButton.GetComponent<ItemButton>();
        itemButton.heldItem = item;
        itemButton.stockNumberText.text = item.stock.ToString();

        newButton.onClick.AddListener(delegate { MoveItem(newButton); });

        return itemButton;
    }

    //Moves item from inventory to shelf
    private void MoveItem(Button interactedButton)
    {
        ItemButton itemButton = interactedButton.GetComponent<ItemButton>();
        ItemInstance itemInstance = itemButton.heldItem;
        int itemStock = playerInventory.GetStock(itemInstance);
        
        if (interactedButton.transform.IsChildOf(playerInventoryPanel.transform))
        {
            if (itemInstance.stock > 1)
            {
                playerInventory.TakeItem(itemInstance);
                if (!activeShelf.heldItems.Contains(itemButton))
                {
                    ItemInstance newItemInstance = new ItemInstance();
                    newItemInstance.item = itemButton.heldItem.item;
                    ItemButton newItemButton = AddNewItemButton(newItemInstance, shelfInventoryPanel);
                    activeShelf.heldItems.Add(newItemButton); 
                }
                else
                {
                    activeShelf.changeStock(itemButton, 1);
                }
            }
            else
            {
                //If it is the last item destroy it after moving it to the other container.
                playerInventory.TakeItem(itemInstance);
                Destroy(interactedButton.gameObject);
            }
        }
        else
        {
            if (itemInstance.stock > 1)
            {
                playerInventory.GiveItem(itemInstance.item);
                activeShelf.changeStock(itemButton, -1);
                int itemIndex = activeShelf.heldItems.IndexOf(itemButton);//May need to check that it actually exists in the list
                int stock = activeShelf.heldItems[itemIndex].heldItem.stock;
                itemButton.stockNumberText.text = stock.ToString();
            }
            else
            {
                playerInventory.GiveItem(itemInstance.item);
                activeShelf.heldItems.Remove(itemButton);
                Destroy(interactedButton.gameObject);
            }

        }
        isModified = true;
    }
       
    public void SetActiveShelf(GameObject newActiveShelf)
    {
        this.activeShelf = newActiveShelf.GetComponent<Shelves>();
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

        foreach (Transform item in shelfInventoryPanel.transform)
        {
            Debug.Log("destroying shelf item buttons");
            Destroy(item.gameObject);
        }

        //If the active shelf has an item on it, make a button for that item and place it.
        foreach (ItemButton itemButton in activeShelf.heldItems)
        {
            Button newButton = Instantiate(itemButtonPrefab);
            newButton.transform.SetParent(shelfInventoryPanel.transform, false);
            Image image = newButton.GetComponent<Image>();
            image.sprite = itemButton.heldItem.item.itemIcon;
            newButton.GetComponent<ItemButton>().heldItem = itemButton.heldItem;
            newButton.onClick.AddListener(delegate { MoveItem(newButton); });
        }
        
        gameObject.SetActive(isShowing);
    }
}

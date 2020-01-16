using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShelfManager : MonoBehaviour
{
    [SerializeField] StoreInventory playerInventory;
    [SerializeField] GameObject ShelfCanvas;
    [SerializeField] GameObject playerInventoryPanel;
    [SerializeField] GameObject shelfInventoryPanel;
    [SerializeField] Button itemButtonPrefab;

    private bool isShowing;
    ItemInstance shelvedItem;
    Shelf activeShelf = null;
    List<ItemInstance> items;
    List<ItemInstance> shelvedItems;//this is what the customers will use to reference what is on the shelves for their purchases.
    private bool isModified = false;

    // Start is called before the first frame update
    void Start()
    {
        shelvedItems = new List<ItemInstance>();
        //For every item in the player inventory, populate the inventory panel with an item button
        items = playerInventory.GetInventory();
        foreach(ItemInstance item in items)
        {
            AddNewItemButton(item, playerInventoryPanel);
        }

        isShowing = false;
        ShelfCanvas.SetActive(isShowing);
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

    public List<ItemInstance> GetShelvedItems()
    {
        return shelvedItems;
    }

    public void SetActiveShelf(GameObject newActiveShelf)
    {
        this.activeShelf = newActiveShelf.GetComponent<Shelf>();
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
                    ShelfItem(newItemButton.heldItem, 1);
                }
                else
                {
                    activeShelf.changeStock(itemButton, 1);
                    ShelfItem(itemButton.heldItem, 1);
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
                int itemIndex = activeShelf.heldItems.IndexOf(itemButton);
                int stock = activeShelf.heldItems[itemIndex].heldItem.stock;
                itemButton.stockNumberText.text = stock.ToString();
            }
            else
            {
                playerInventory.GiveItem(itemInstance.item);
                activeShelf.heldItems.Remove(itemButton);
                Destroy(interactedButton.gameObject);
                shelvedItems.Remove(itemButton.heldItem);
            }

        }
        isModified = true;
    }

    private void ShelfItem(ItemInstance heldItem, int amount)
    {
        ItemInstance itemReference = new ItemInstance(heldItem);
        itemReference.stock = amount;
        
        shelvedItems.Add(itemReference);
        itemReference.shelf = activeShelf;
        
    }

    public void HideOrShow()
    {
        isShowing = !isShowing;

        foreach (Transform item in shelfInventoryPanel.transform)
        {
            Destroy(item.gameObject);
        }

        //If the active shelf has an item on it, make a button for that item and place it.
        foreach (ItemButton itemButton in activeShelf.heldItems)
        {
            Button newButton = Instantiate(itemButtonPrefab);
            newButton.transform.SetParent(shelfInventoryPanel.transform, false);
            Image image = newButton.GetComponent<Image>();
            image.sprite = itemButton.heldItem.item.itemIcon;

            ItemButton newItemButton = newButton.GetComponent<ItemButton>();
            newItemButton.heldItem = itemButton.heldItem;
            newItemButton.stockNumberText.text = itemButton.heldItem.stock.ToString();
            newButton.onClick.AddListener(delegate { MoveItem(newButton); });
        }
        
        ShelfCanvas.SetActive(isShowing);
    }
}

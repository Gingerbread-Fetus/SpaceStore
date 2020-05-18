using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class made for assisting abilities in selecting one item from the inventory.
/// </summary>
public class AbilityInventoryHandler : MonoBehaviour
{
    [SerializeField] StoreInventory playerInventory;
    [SerializeField] GameObject playerInventoryPanel;
    [SerializeField] GameObject selectedItemPanel;
    [SerializeField] Button itemButtonPrefab;
    [SerializeField] TextMeshProUGUI flavorTextObject;
    [HideInInspector] public ItemInstance selectedItem;

    GameObject miniGameObject;

    List<ItemInstance> items;

    public GameObject MiniGameObject { get => miniGameObject; set => miniGameObject = value; }

    // Start is called before the first frame update
    void Start()
    {
        CreateInventoryButtons();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void CreateInventoryButtons()
    {
        items = playerInventory.GetInventory();
        foreach (ItemInstance item in items)
        {
            AddNewItemButton(item, playerInventoryPanel);
        }
    }

    private void UpdateButtons()
    {
        foreach (ItemButton itemButton in playerInventoryPanel.transform.GetComponentsInChildren<ItemButton>())
        {
        }
    }

    private void AddNewItemButton(ItemInstance item, GameObject parentPanel)
    {
        Button newButton = Instantiate(itemButtonPrefab);
        newButton.transform.SetParent(parentPanel.transform, false);
        Image image = newButton.GetComponent<Image>();
        image.sprite = item.item.itemIcon;

        ItemButton itemButton = newButton.GetComponent<ItemButton>();
        itemButton.heldItem = item;

        newButton.onClick.AddListener(delegate { SelectItem(itemButton.heldItem, newButton); });
    }

    /// <summary>
    /// </summary>
    /// <param name="heldItem"></param>
    /// <param name="interactedButton"></param>
    private void SelectItem(ItemInstance heldItem, Button interactedButton)
    {
        ItemButton itemButton = interactedButton.GetComponent<ItemButton>();
        ItemInstance itemInstance = itemButton.heldItem;

        ItemInstance newItem = new ItemInstance(itemInstance);
        newItem.stock = 1;

        if (interactedButton.transform.IsChildOf(playerInventoryPanel.transform))
        {
            selectedItem = heldItem;
            if(selectedItemPanel.transform.childCount > 0)
            {
                //Take desired item from player inventory
                playerInventory.TakeItem(itemInstance);
                //Get the item currently on the item panel.
                GameObject existingItem = selectedItemPanel.transform.GetChild(0).gameObject;
                ItemInstance itemOnPanel = existingItem.GetComponent<ItemButton>().heldItem;
                //Return item to player, dispose of button, create new button in its place.
                playerInventory.GiveItem(itemOnPanel.item);
                UpdateButtons();
                Destroy(selectedItemPanel.transform.GetChild(0).gameObject);
                AddNewItemButton(newItem, selectedItemPanel);
                selectedItem = newItem;
            }
            else
            {
                playerInventory.TakeItem(itemInstance);
                AddNewItemButton(newItem, selectedItemPanel);
                selectedItem = newItem;
                UpdateButtons();
            }
        }
        else
        {
            playerInventory.GiveItem(heldItem.item);
            Destroy(interactedButton.gameObject);
            selectedItem = null;
            UpdateButtons();
        }
    }

    public void SetFlavortext(string newText)
    {
        flavorTextObject.text = newText;
    }

    public void Confirm()
    {
        if (selectedItem != null)
        {
            gameObject.SetActive(false); 
        }
    }
    
}

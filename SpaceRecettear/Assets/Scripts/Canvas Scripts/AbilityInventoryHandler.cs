﻿using System;
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
    [SerializeField] Ability ability;
    [HideInInspector] public ItemInstance selectedItem;

    GameObject miniGameObject;

    List<ItemInstance> items;

    public GameObject MiniGameObject { get => miniGameObject; set => miniGameObject = value; }

    // Start is called before the first frame update
    void Start()
    {
        items = playerInventory.GetInventory();
        foreach (ItemInstance item in items)
        {
            AddNewItemButton(item, playerInventoryPanel);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddNewItemButton(ItemInstance item, GameObject parentPanel)
    {
        Button newButton = Instantiate(itemButtonPrefab);
        newButton.transform.SetParent(parentPanel.transform, false);
        Image image = newButton.GetComponent<Image>();
        image.sprite = item.item.itemIcon;

        ItemButton itemButton = newButton.GetComponent<ItemButton>();
        itemButton.heldItem = item;
        itemButton.stockNumberText.text = item.stock.ToString();

        newButton.onClick.AddListener(delegate { SelectItem(itemButton.heldItem, newButton); });
    }

    /// <summary>
    /// TODO: Implement decreasing inventory here.
    /// TODO: Implement swaping inventory back and forth.
    /// </summary>
    /// <param name="heldItem"></param>
    /// <param name="interactedButton"></param>
    private void SelectItem(ItemInstance heldItem, Button interactedButton)
    {
        ItemButton itemButton = interactedButton.GetComponent<ItemButton>();
        ItemInstance itemInstance = itemButton.heldItem;
        int itemStock = playerInventory.GetStock(itemInstance);

        if (interactedButton.transform.IsChildOf(playerInventoryPanel.transform))
        {
            selectedItem = heldItem;
            if(selectedItemPanel.transform.childCount > 0)
            {
                //swap
                Destroy(selectedItemPanel.transform.GetChild(0).gameObject);
                AddNewItemButton(itemInstance, selectedItemPanel);
                selectedItem = heldItem;
            }
            else
            {
                AddNewItemButton(itemInstance, selectedItemPanel);
                selectedItem = heldItem;
            }
        }
        else
        {
            Destroy(interactedButton.gameObject);
            selectedItem = null;
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
            ability.TriggerAbility();
            gameObject.SetActive(false); 
        }
    }

    public void SetAbility(Ability newAbility)
    {
        ability = newAbility;
    }
    
}

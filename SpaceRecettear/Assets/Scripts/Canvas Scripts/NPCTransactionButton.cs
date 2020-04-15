using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class NPCTransactionButton : MonoBehaviour
{
    Image buttonSprite;
    TextMeshProUGUI stockText;
    Button buttonComponent;

    ItemInstance heldItem;
    public ItemInstance HeldItem { get => heldItem; set => heldItem = value; }

    // Start is called before the first frame update
    void Start()
    {
        buttonComponent = GetComponent<Button>();
        buttonSprite = GetComponent<Image>();
        stockText = GetComponentInChildren<TextMeshProUGUI>();
        buttonSprite.sprite = heldItem.item.itemIcon;
        buttonComponent.onClick.AddListener(delegate { ButtonClicked(heldItem); });
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUIDetails();
    }

    private void UpdateUIDetails()
    {
        if (heldItem.isLimited)
        {
            stockText.text = heldItem.stock.ToString(); 
            if (heldItem.stock == 0)
            {
                buttonComponent.interactable = false;
            }
            else
            {
                buttonComponent.interactable = true;
            }
        }
        else
        {
            stockText.gameObject.SetActive(false);
        }
    }

    private void ButtonClicked(ItemInstance heldItem)
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            SendToShoppingCart(5);
        }
        else if(Input.GetKey(KeyCode.LeftShift))
        {
            SendToShoppingCart(100);
        }
        else
        {
            SendToShoppingCart(1);
        }
    }

    private void SendToShoppingCart(int amountToSend)
    {
        ShoppingCartPanel shoppingCartPanel = FindObjectOfType<ShoppingCartPanel>();
        ItemInstance newItemInstance = new ItemInstance(heldItem);
        if (amountToSend < heldItem.stock)
        {
            newItemInstance.stock = amountToSend; 
        }
        else
        {
            newItemInstance.stock = heldItem.stock;
        }
        heldItem.stock -= newItemInstance.stock;
        shoppingCartPanel.AddItemToShoppingCart(newItemInstance);
    }
}
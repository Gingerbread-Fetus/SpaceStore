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
    [SerializeField] Button itemButtonPrefab;
    [SerializeField] TextMeshProUGUI flavorTextObject;

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

    private void AddNewItemButton(ItemInstance item, GameObject playerInventoryPanel)
    {
        Button newButton = Instantiate(itemButtonPrefab);
        newButton.transform.SetParent(playerInventoryPanel.transform, false);
        Image image = newButton.GetComponent<Image>();
        image.sprite = item.item.itemIcon;

        ItemButton itemButton = newButton.GetComponent<ItemButton>();
        itemButton.heldItem = item;
        itemButton.stockNumberText.text = item.stock.ToString();

        newButton.onClick.AddListener(delegate { SelectItem(itemButton.heldItem); });
    }

    private void SelectItem(ItemInstance heldItem)
    {
        selectedItem = heldItem;
    }

    public void SetFlavortext(string newText)
    {
        flavorTextObject.text = newText;
    }
    
}

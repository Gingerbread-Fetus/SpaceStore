using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : Furniture, IInteractable
{
    [SerializeField] public List<ItemButton> heldItems;
    [SerializeField] public SpriteRenderer spriteHolder;
    ShelfManager shelfManager;
    
    // Start is called before the first frame update
    void Start()
    {
        heldItems = new List<ItemButton>();
        shelfManager = FindObjectOfType<ShelfManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (heldItems.Count > 0)
        {
            spriteHolder.sprite = heldItems[0].heldItem.item.itemIcon; 
        }
        else
        {
            spriteHolder.sprite = null;
        }
    }

    public override int CalculateSellPrice()
    {
        throw new System.NotImplementedException();
    }

    public void Interact()
    {
        Debug.Log("Shelf " + name + " interacted with");
        //Open shelf UI
        shelfManager.SetActiveShelf(this.gameObject);
        shelfManager.HideOrShow();
    }
    
    public void changeStock(ItemButton item, int stockChange)
    {
        int itemIndex = heldItems.IndexOf(item);
        if (itemIndex >= 0)
        {
            heldItems[itemIndex].heldItem.stock += stockChange;
        }
    }
}

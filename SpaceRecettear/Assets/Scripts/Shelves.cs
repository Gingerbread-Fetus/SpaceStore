using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelves : Furniture, IInteractable
{
    [SerializeField] public List<ItemButton> heldItems;
    ShelfManager shelfManager;

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

    // Start is called before the first frame update
    void Start()
    {
        heldItems = new List<ItemButton>();
        shelfManager = FindObjectOfType<UIManager>().shelfManager;
    }

    public void changeStock(ItemButton item ,int stockChange)
    {
        int itemIndex = heldItems.IndexOf(item);
        if(itemIndex >= 0)
        {
            heldItems[itemIndex].heldItem.stock += stockChange;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

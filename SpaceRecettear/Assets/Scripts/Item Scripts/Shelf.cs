using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : Furniture, IInteractable
{
    [SerializeField] public List<ItemInstance> heldItems = new List<ItemInstance>();//TODO: I don't like what this does in the editor, and it makes some testing harder, I should change it.
    [SerializeField] public SpriteRenderer spriteHolder;
    [SerializeField] public GameObject standArea;

    ShelfManager shelfManager;
    CustomerManager customerManager;
    Collider2D standAreaCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        shelfManager = FindObjectOfType<ShelfManager>();
        customerManager = FindObjectOfType<CustomerManager>();
        standAreaCollider = standArea.GetComponent<CircleCollider2D>();
        if(heldItems.Count > 0)
        {
            foreach (ItemInstance item in heldItems)
            {
                item.Shelf = this;
                customerManager.UnclaimItem(item);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (heldItems.Count > 0)
        {
            spriteHolder.sprite = heldItems[0].item.itemIcon;
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
        Debug.Log(name + " interacted with");
        //Open shelf UI
        shelfManager.SetActiveShelf(this.gameObject);
        shelfManager.HideOrShow();
    }
    
    public void changeStock(ItemInstance item, int stockChange)
    {
        int itemIndex = heldItems.IndexOf(item);
        if (itemIndex >= 0)
        {
            heldItems[itemIndex].stock += stockChange;
            if(heldItems[itemIndex].stock <= 0) { heldItems.Remove(item); }
        }
    }

    /// <summary>
    /// This method returns the position of the shelf stand object. That being 
    /// where we want the customers to stand.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPosition()
    {
        return standArea.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name += " has entered a shelf collider");
    }
}

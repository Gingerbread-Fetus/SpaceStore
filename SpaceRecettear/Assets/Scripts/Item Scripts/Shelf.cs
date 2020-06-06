using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : Furniture, IInteractable
{
    [SerializeField] ItemInstance heldItem;
    [SerializeField] public List<ItemInstance> heldItems = new List<ItemInstance>();
    [SerializeField] public SpriteRenderer spriteHolder;
    [SerializeField] public GameObject standArea;

    ShelfManager shelfManager;
    CustomerDirector customerDirector;
    Collider2D standAreaCollider;

    public ItemInstance HeldItem
    {
        get
        {
            return heldItem;
        }
        set
        {
            if (value == null)
            {
                customerDirector.RemoveFromClaimedShelves(this);
            }
            heldItem = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        shelfManager = FindObjectOfType<ShelfManager>();
        customerDirector = FindObjectOfType<CustomerDirector>();
        standAreaCollider = standArea.GetComponent<CircleCollider2D>();
        
        if(HeldItem.item != null)
        {
            //TODO, rework code so that shelves only hold one item.
            customerDirector.UnclaimShelf(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (heldItem.item != null)
        {
            spriteHolder.sprite = heldItem.item.itemIcon;
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

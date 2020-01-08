using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelves : Furniture, IInteractable
{
    [SerializeField] ItemInstance heldItem;
    public override int CalculateSellPrice()
    {
        throw new System.NotImplementedException();
    }

    public void Interact()
    {
        Debug.Log("Shelf " + name + " interacted with");
        //Open shelf UI
        ShelfManager shelfManager = FindObjectOfType<UIManager>().shelfManager;
        shelfManager.HideOrShow();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

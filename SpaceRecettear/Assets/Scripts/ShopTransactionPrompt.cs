using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopTransactionPrompt : MonoBehaviour
{
    public enum TransactionType
    {
        TakeFromShop,
        GiveToShop
    }

    [SerializeField] Image itemImageHolder;
    [SerializeField] TMP_InputField stockInputField;

    public TransactionType transactionState = TransactionType.TakeFromShop;
    ShopkeepManager shopkeepManager;
    GuildShopTransactionButton desiredItem;
    int desiredItemStock = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        shopkeepManager = GetComponentInParent<ShopkeepManager>();
        desiredItemStock = 0;
        stockInputField.text = desiredItemStock.ToString();
    }

    public void setDesiredItem(GuildShopTransactionButton newDesiredItem)
    {
        desiredItem = newDesiredItem;
        itemImageHolder.sprite = desiredItem.heldItem.item.itemIcon;
    }

    public void AddToStock(int stockChange)
    {
        desiredItemStock += stockChange;
        stockInputField.text = desiredItemStock.ToString();
    }

    public void SetDesiredStock()
    {
        int newStock = int.Parse(stockInputField.text);
    }

    public void SendStockToTransaction()
    {
        if (transactionState == TransactionType.TakeFromShop)
        {
            shopkeepManager.MoveItemToTransaction(desiredItem, desiredItemStock); 
        }
        else if(transactionState == TransactionType.GiveToShop)
        {
            shopkeepManager.RemoveItemFromTransaction(desiredItem, desiredItemStock);
        }
        gameObject.SetActive(false);
    }
}

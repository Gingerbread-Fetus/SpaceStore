using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopTransactionPrompt : MonoBehaviour
{
    [SerializeField] Image itemImageHolder;
    [SerializeField] TMP_InputField stockInputField;

    ShopkeepManager shopkeepManager;
    ItemInstance desiredItem;
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
    }

    public void setDesiredItem(ItemInstance newDesiredItem)
    {
        desiredItem = newDesiredItem;
        itemImageHolder.sprite = desiredItem.item.itemIcon;
    }

    public void AddToStock(int newAmount)
    {
        desiredItemStock += newAmount;
        stockInputField.text = newAmount.ToString();
    }

    public void SetDesiredStock()
    {
        int newStock = int.Parse(stockInputField.text);
    }

    public void SendStockToTransaction()
    {
        desiredItem.stock = desiredItemStock;
        shopkeepManager.MoveItemToTransaction(desiredItem);
        gameObject.SetActive(false);
    }
}

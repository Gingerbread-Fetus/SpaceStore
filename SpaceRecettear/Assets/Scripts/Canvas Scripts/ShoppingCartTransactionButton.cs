using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingCartTransactionButton : MonoBehaviour
{
    Image buttonSprite;
    TextMeshProUGUI stockText;
    Button buttonComponent;

    ItemInstance heldItem;
    public ItemInstance HeldItem { get => heldItem; set => heldItem = value; }

    // Start is called before the first frame update
    void Start()
    {
        Button buttonComponent = GetComponent<Button>();
        buttonSprite = GetComponent<Image>();
        stockText = GetComponentInChildren<TextMeshProUGUI>();
        buttonComponent.onClick.AddListener(delegate { RemoveFromCart(heldItem); });
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUIDetails();
    }

    private void UpdateUIDetails()
    {
        buttonSprite.sprite = heldItem.item.itemIcon;
        
        if (heldItem.stock >= 1)
        {
            stockText.text = heldItem.stock.ToString();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void RemoveFromCart(ItemInstance heldItem)
    {
        ShoppingCartPanel shoppingCartPanel = FindObjectOfType<ShoppingCartPanel>();
        if (Input.GetKey(KeyCode.LeftControl))
        {
            shoppingCartPanel.RemoveFromShoppingCart(this, 5);
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            shoppingCartPanel.RemoveFromShoppingCart(this, 100);
        }
        else
        {
            shoppingCartPanel.RemoveFromShoppingCart(this, 1);
        }
    }

}

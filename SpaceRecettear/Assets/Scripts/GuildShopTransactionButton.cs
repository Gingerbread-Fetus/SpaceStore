using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuildShopTransactionButton : MonoBehaviour
{
    public ItemInstance heldItem;
    [SerializeField] TextMeshProUGUI stockNumberText;
    [SerializeField] bool limitedItem = false;

    void Update()
    {
        if (!limitedItem)
        {
            stockNumberText.text = heldItem.stock.ToString(); 
        }
    }

    void Start()
    {
        //HELP
    }

    public override bool Equals(object obj)
    {
        if (obj == null || this.GetType() != obj.GetType()) { return false; }
        GuildShopTransactionButton c = obj as GuildShopTransactionButton;
        return this.heldItem.Equals(c.heldItem);
    }

    public override int GetHashCode()
    {
        var hashCode = -175335629;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<ItemInstance>.Default.GetHashCode(heldItem);
        return hashCode;
    }
}

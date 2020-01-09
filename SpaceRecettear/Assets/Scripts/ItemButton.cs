using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    public ItemInstance heldItem;
    [SerializeField]public TextMeshProUGUI stockNumberText;

    public override bool Equals(object obj)
    {
        if (obj == null || this.GetType() != obj.GetType()) { return false; }
        ItemButton c = obj as ItemButton;
        return this.heldItem.Equals(c.heldItem);
    }
}

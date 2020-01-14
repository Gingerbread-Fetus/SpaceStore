using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Customer Profile", menuName = "Customers/Profile")]
public class CustomerProfile : ScriptableObject
{
    [SerializeField] public List<ItemInstance> favoriteItems;
    [SerializeField] public Sprite customerSprite;
    [SerializeField] public AnimatorOverrideController animatorController;
}

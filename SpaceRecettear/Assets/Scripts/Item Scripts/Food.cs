using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable ,CreateAssetMenu  (fileName = "new Food", menuName = "Items/Food")]
public class Food : Item
{
    public enum FoodType
    {
        Bread, Apple, Cake
    }

    [SerializeField] FoodType foodType;
}


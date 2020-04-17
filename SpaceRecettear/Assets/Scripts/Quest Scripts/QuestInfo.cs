using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest")]
public class QuestInfo : ScriptableObject
{
    [SerializeField] public List<ItemRewardBundle> itemRewards = new List<ItemRewardBundle>();
    [TextArea(10, 14)] [SerializeField] public string storyText;
    [Range(0,10)][SerializeField] public int difficulty = 0;
    [Range(1,10)][SerializeField] public int numberOfAdventurers = 1;
    public List<CustomerProfile> recommendedAdventurers;

    public float GetSuccessChance()
    {
        return 1f;
    }


    [Serializable]
    public class ItemRewardBundle
    {
        [Range(1,9999)] [SerializeField] public int amount = 1;
        [SerializeField] public Item item;
    }
}

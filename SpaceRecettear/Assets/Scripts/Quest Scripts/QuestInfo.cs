using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest")]
public class QuestInfo : ScriptableObject
{
    [SerializeField] List<ItemRewardBundle> itemRewards = new List<ItemRewardBundle>();
    [TextArea(10, 14)] [SerializeField] public string storyText;
    [Range(0,10)][SerializeField] public int difficulty = 0;

    public float GetSuccessChance()
    {
        return 1f;
    }


    [Serializable]
    private class ItemRewardBundle
    {
        [Range(1,9999)] [SerializeField] int amount;
        [SerializeField] Item item;
    }
}

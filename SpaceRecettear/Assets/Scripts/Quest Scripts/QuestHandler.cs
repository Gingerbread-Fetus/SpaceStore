using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    [SerializeField] QuestInfo selectedQuest;
    [SerializeField] TextMeshProUGUI questTextPanel;
    [SerializeField] TextMeshProUGUI questInfoPanel;
    // Start is called before the first frame update
    void Start()
    {
        ChangeQuestText();
        ChangeQuestInfo();
    }

    private void ChangeQuestText()
    {
        questTextPanel.text = selectedQuest.storyText;
    }

    private void ChangeQuestInfo()
    {
        questInfoPanel.text = "The quest requires at least: " + selectedQuest.numberOfAdventurers + " adventurers.\n";
        questInfoPanel.text += "The quest difficulty is: " + selectedQuest.difficulty + ".\n";
        PrintRecommendedAdventurers();
        PrintRewards();
    }

    private void PrintRecommendedAdventurers()
    {
        questInfoPanel.text += "Adventurers recommended for this quest:";
        foreach(CustomerProfile profile in selectedQuest.recommendedAdventurers)
        {
            questInfoPanel.text += " " + profile.characterName + ", ";
        }
        questInfoPanel.text += "\n";
    }

    private void PrintRewards()
    {
        questInfoPanel.text += "Rewards: ";
        foreach(var itemBundle in selectedQuest.itemRewards)
        {
            questInfoPanel.text += itemBundle.item.name + " x " + itemBundle.amount + ", ";
        }
        questInfoPanel.text += "\n";
    }
}

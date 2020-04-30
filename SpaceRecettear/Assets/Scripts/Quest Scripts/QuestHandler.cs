using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestHandler : MonoBehaviour
{
    [SerializeField] QuestInfo[] availableQuests;
    [SerializeField] CustomerProfile[] availableAdventurers;//todo how do we make adventurers available
    [SerializeField] TextMeshProUGUI questTextPanel;
    [SerializeField] TextMeshProUGUI questInfoPanel;
    [SerializeField] QuestButton questButtonPrefab;//todo consider making the prefab a meber of the quest class and instantiating it from there.
    [SerializeField] Transform questButtonPanel;

    QuestInfo selectedQuest;

    // Start is called before the first frame update
    void Start()
    {
        selectedQuest = availableQuests[0];
        ChangeQuestText();
        ChangeQuestInfo();
        CreateQuestButtons();
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

    private void CreateQuestButtons()
    {
        foreach (QuestInfo quest in availableQuests)
        {
            QuestButton questButton = Instantiate<QuestButton>(questButtonPrefab, questButtonPanel);
            questButton.quest = quest;
            questButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = quest.questName;
            questButton.GetComponent<Button>().onClick.AddListener(delegate { SetSelectedQuest(questButton.quest); });
        }
    }

    private void SetSelectedQuest(QuestInfo quest)
    {
        selectedQuest = quest;
        ChangeQuestInfo();
        ChangeQuestText();
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

    public CustomerProfile[] GetAvailableAdventurers()
    {
        return availableAdventurers;
    }
}

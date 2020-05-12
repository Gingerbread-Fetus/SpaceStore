using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HagglingCanvas : MonoBehaviour
{
    [SerializeField] HagglingController hagglingController;
    [SerializeField] OfferChanger offerChanger;
    [SerializeField] SatisfactionBubble satisfactionBubble;
    [SerializeField] Slider staminaSlider;
    [SerializeField] Image desiredItemImage;
    [SerializeField] TextMeshProUGUI itemValuetext;
    [SerializeField] TextMeshProUGUI playerGoldText;
    [SerializeField] TextMeshProUGUI offerText;
    [SerializeField] GameObject abilityPanel;
    [SerializeField] AbilityButton abilityButtonPrefab;

    public GameObject AbilityPanel { get => abilityPanel; }

    void Start()
    {
        staminaSlider.maxValue = hagglingController.PlayerProfile.GetStamina();
        staminaSlider.value = staminaSlider.maxValue;
        PopulateAbilityButtons();
        abilityPanel.SetActive(false);
        gameObject.SetActive(false);
    }

    public void RedrawAll()
    {
        desiredItemImage.sprite = hagglingController.DesiredItem.item.itemIcon;
        itemValuetext.text = hagglingController.transaction.GetValue().ToString();
        playerGoldText.text = hagglingController.PlayerInventory.GetCurrency().ToString();
        offerText.text = hagglingController.transaction.Offer.ToString();
        staminaSlider.value = hagglingController.CurrentStamina;
    }

    public void ToggleDealPanelActive()
    {
        if (abilityPanel.activeInHierarchy) { abilityPanel.SetActive(false); }
        else { abilityPanel.SetActive(true); }
    }

    private void PopulateAbilityButtons()
    {
        List<Ability> abilityList = hagglingController.PlayerProfile.Abilities;
        foreach(Ability ability in abilityList)
        {
            Debug.Log("Adding Ability: " + ability.aName);
            var newButton = Instantiate(abilityButtonPrefab, abilityPanel.transform);
            newButton.SetAbility(ability);
            //TODO: hook up the new buttons on click event to the relevant ability
        }
    }
}

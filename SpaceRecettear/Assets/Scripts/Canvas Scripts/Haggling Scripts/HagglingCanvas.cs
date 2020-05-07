using System;
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

    void Start()
    {
        staminaSlider.maxValue = hagglingController.PlayerProfile.GetStamina();
        staminaSlider.value = staminaSlider.maxValue;
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
}

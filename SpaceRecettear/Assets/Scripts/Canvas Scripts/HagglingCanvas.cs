using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HagglingCanvas : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI offerText;
    [SerializeField] TextMeshProUGUI transactionValueText;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] int maxStamina = 50;
    [SerializeField] Slider staminaBar;

    int currentStamina;
    PlayerProfile playerProfile;
    StoreInventory playerInventory;

    [HideInInspector] public int currentOffer;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);

        if (isActiveAndEnabled)
        {
            //Set up UI
            offerText.text = currentOffer.ToString();
            transactionValueText.text = currentOffer.ToString();
            goldText.text = playerInventory.GetCurrency().ToString();
            maxStamina = playerProfile.GetStamina();
            currentStamina = maxStamina;
            staminaBar.maxValue = maxStamina;
            staminaBar.value = maxStamina; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using TMPro;
using UnityEngine;

public class OfferChanger : MonoBehaviour
{
    [SerializeField] TMP_InputField textField;

    int startingOffer;
    int resultingOffer;
    int offerChange = 1;
    float startTime;
    // Start is called before the first frame update
    void Start()
    {
        textField.text = startingOffer.ToString();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                startTime = 0;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                IncreaseOffer(offerChange);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                DecreaseOffer(offerChange);
            }
        }
    }

    public void Show()
    {
        HagglingController hagglingController = FindObjectOfType<HagglingController>();
        if (hagglingController.CurrentStamina > 0)
        {
            startingOffer = hagglingController.transaction.Offer;
            hagglingController.CurrentStamina -= hagglingController.HagglingCost;
            textField.text = startingOffer.ToString();
            gameObject.SetActive(true); 
        }
        else
        {
            Debug.Log("Stamina too low");
        }
    }

    public void Done()
    {
        HagglingController hagglingController = FindObjectOfType<HagglingController>();
        hagglingController.transaction.Offer = resultingOffer;
        hagglingController.CalculateSatisfaction();
        FindObjectOfType<HagglingCanvas>().RedrawAll();
        gameObject.SetActive(false);
    }

    public void IncreaseOffer(int offerDifference)
    {
        resultingOffer += offerDifference;
        textField.text = resultingOffer.ToString();
    }

    public void DecreaseOffer(int offerDifference)
    {
        resultingOffer -= offerDifference;
        textField.text = resultingOffer.ToString();
    }

    public void SetOffer(int newOffer)
    {
        resultingOffer = newOffer;
        textField.text = resultingOffer.ToString();
    }

    public void SetOfferFromValueChange()
    {
        resultingOffer = int.Parse(textField.text);
    }
}

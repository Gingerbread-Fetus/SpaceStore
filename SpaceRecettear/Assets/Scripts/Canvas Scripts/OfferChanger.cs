using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OfferChanger : MonoBehaviour
{
    [SerializeField] TMP_InputField textField;

    HagglingManager hagglingManager;
    int startingOffer;
    int resultingOffer;
    int offerChange = 1;
    float startTime;
    // Start is called before the first frame update
    void Start()
    {
        hagglingManager = FindObjectOfType<HagglingManager>();
        startingOffer = hagglingManager.currentOffer;
        resultingOffer = hagglingManager.currentOffer;
        textField.text = startingOffer.ToString();
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
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Done()
    {
        hagglingManager.currentOffer = resultingOffer;
        hagglingManager.currentTransaction.Offer = resultingOffer;
        hagglingManager.ExitHagglingMenu();
        Hide();
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

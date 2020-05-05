using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HagglingController : MonoBehaviour
{
    private bool introShown;
    private bool isAbilitiesVisible = false;
    private int moneyChange = 0;
    private int currentStamina;
    OfferChanger offerChanger;
    ClosingManager closingManager;
    CustomerManager customerManager;
    private ItemInstance itemForSale;
    [SerializeField] private CustomerController activeCustomer;//Serialized for debugging
    private CustomerProfile activeCustomerProfile;

    [Header("Game Session Properties")]
    [SerializeField] GameObject customerManagerObject;
    [SerializeField] PlayerController playerController;
    [SerializeField] float gameSpeed;
    [HideInInspector] public int currentOffer;
    [SerializeField] bool playerTurn;
    [Header("Inventory references")]
    [SerializeField] StoreInventory playerInventory;
    [SerializeField] PlayerProfile playerProfile;
    [SerializeField] StoreInventory customerInventory;
    [Header("Canvas References")]
    [SerializeField] GameObject hagglingCanvas;
    [SerializeField] GameObject closingCanvas;
    [SerializeField] GameObject storeOverlay;
    [Header("Transaction")]
    [SerializeField] public Transaction currentTransaction;
    [Header("Encounter Variables")]
    [SerializeField] private int haggleCost = 5;
    private bool isCanvasActive = false;

    public int Stamina { get => currentStamina; set => currentStamina = value; }

    // Start is called before the first frame update
    void Start()
    {
        closingManager = closingCanvas.GetComponent<ClosingManager>();
        customerManager = customerManagerObject.GetComponent<CustomerManager>();
        closingManager.StartingCash = playerInventory.GetCurrency();
    }

    // Update is called once per frame
    void Update()
    {
        if (customerManager.WaveFinished)
        {
            ShowEndOfShift();
        }
    }

    public void ExitHagglingMenu()
    {
        print("This method still needs to be moved.");
    }


    public void ToggleCanvas()
    {
        isCanvasActive = !isCanvasActive;
        hagglingCanvas.SetActive(isCanvasActive);
    }

    public void HideCanvas()
    {
        hagglingCanvas.SetActive(false);
        playerController.controlActive = true;
    }

    public void SetActiveCustomer(GameObject gameObject)
    {
        activeCustomer = gameObject.GetComponent<CustomerController>();
        activeCustomerProfile = activeCustomer.customerProfile;
    }

    public void ShowCanvas()
    {
        playerController.controlActive = false;
        //Get and set up the item for sale.
        itemForSale = activeCustomer.desiredItem;
        //currentOffer = itemForSale.CalculateItemPrice();//This resets the offer amount.
        hagglingCanvas.GetComponent<HagglingCanvas>().SetItemForSale(itemForSale);
        activeCustomerProfile.SetTransaction(currentTransaction);
        hagglingCanvas.SetActive(true);
    }

    public void ShowEndOfShift()
    {
        //Make sure to hide other canvases
        storeOverlay.gameObject.SetActive(false);
        closingCanvas.SetActive(true);
        closingManager.EndingCash = playerInventory.GetCurrency();
        closingManager.EndOfSession();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClosingManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI startingCashText;
    [SerializeField] TextMeshProUGUI endingCashText;
    [SerializeField] TextMeshProUGUI totalSaleText;
    [SerializeField] TextMeshProUGUI totalProfitText;

    int startingCash;
    int endingCash;
    int totalSales;
    int totalProfit;

    public int StartingCash { get => startingCash; set => startingCash = value; }
    public int EndingCash { get => endingCash; set => endingCash = value; }
    public int TotalProfit { get => totalProfit; set => totalProfit = value; }
    public int TotalSales { get => totalSales; set => totalSales = value; }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndOfSession()
    {
        TotalProfit = endingCash - startingCash;

        //Show the stats for that day
        startingCashText.text = StartingCash.ToString();
        endingCashText.text = EndingCash.ToString();
        totalSaleText.text = TotalSales.ToString();
        totalProfitText.text = TotalProfit.ToString();
        //Give player a grade?
    }

    /// <summary>
    /// Plan to make this method the method where I make a call to save changes to inventory
    /// and other things. For now I'm just going to make a call back to the 'title' scene.
    /// </summary>
    public void EndShift()
    {
        SceneManager.LoadScene(0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] CustomerPool customerPool;
    [SerializeField] GameObject customerPrefab;
    [SerializeField] GameObject entrance;
    [SerializeField] GameObject progressSliderGameObject;
    [SerializeField] float spawnWait = 5f;
    [SerializeField] private ShelfManager shelfManager;
    [SerializeField] int waveSize;//Number of customers to spawn

    bool waveFinished = false;

    public List<ItemInstance> claimedItems;
    public List<ItemInstance> unclaimedItems;
    public bool waveSpawning = false;
    
    Slider progressSlider;

    public int WaveSize { get => waveSize;}
    public bool WaveFinished { get => waveFinished;}

    // Start is called before the first frame update
    void Start()
    {
        shelfManager = FindObjectOfType<ShelfManager>();
        progressSlider = progressSliderGameObject.GetComponent<Slider>();
        progressSlider.maxValue = waveSize;
        progressSlider.value = waveSize;
        if (unclaimedItems == null)
        {
            unclaimedItems = new List<ItemInstance>(); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        progressSlider.value = waveSize;
        //If the wave has not already finished, and the wave has all been spawned, and all children have bee destroyed, end wave
        if(!waveFinished && waveSize <= 0 && transform.childCount == 0)
        {
            waveFinished = true;
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnCustomers());
    }

    public IEnumerator SpawnCustomers()
    {
        while (waveSpawning && waveSize >= 0)
        {
            //Get a random profile from the pool
            CustomerProfile customerProfile = customerPool.GetRandomProfile();

            if (customerProfile != null)
            {
                GameObject newCustomer = Instantiate(customerPrefab, entrance.transform.position, Quaternion.identity,gameObject.transform);
                CustomerController newCustomerController = newCustomer.GetComponent<CustomerController>();
                newCustomerController.customerProfile = customerProfile;
                newCustomerController.levelExit = entrance;
                waveSize--;
                yield return new WaitForSecondsRealtime(spawnWait);
            }
        }
    }

    public ItemInstance ClaimItem(ItemInstance item)
    {
        int itemIndex = 0;
        List<int> itemIndices = new List<int>();
        ItemInstance claimedItem = null;
        if (unclaimedItems.Contains(item))
        {
            foreach (ItemInstance itemInstance in unclaimedItems)
            {
                if (itemInstance.Equals(item))
                {
                    itemIndices.Add(itemIndex);
                    itemIndex++;
                }
            }
            //.Remove uses the equals method. If there are duplicate items, this is a problem because it will just remove the first occurence of the item
            int randomIndex = itemIndices[Random.Range(0, itemIndices.Count - 1)];
            claimedItem = unclaimedItems[randomIndex];
            unclaimedItems.RemoveAt(randomIndex);
            waveSpawning = unclaimedItems.Count > 0;
        }
        if (claimedItem != null)
        {
            claimedItems.Add(item); 
        }
        return claimedItem;
    }

    public void UnclaimItem(ItemInstance item)
    {
        if (claimedItems.Contains(item))
        {
            claimedItems.Remove(item); 
        }
        unclaimedItems.Add(item);
        waveSpawning = unclaimedItems.Count > 0;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] CustomerPool customerPool;
    [SerializeField] GameObject customerPrefab;
    [SerializeField] GameObject entrance;
    [SerializeField] float spawnWait = 5f;
    [SerializeField] private ShelfManager shelfManager;

    public List<ItemInstance> claimedItems;
    public List<ItemInstance> unclaimedItems;
    public bool waveSpawning;

    
    // Start is called before the first frame update
    void Start()
    {
        shelfManager = FindObjectOfType<ShelfManager>();
        if (unclaimedItems == null)
        {
            unclaimedItems = new List<ItemInstance>(); 
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator SpawnCustomers()
    {
        waveSpawning = true;
        while (waveSpawning == true)
        {
            foreach (CustomerProfile customerProfile in customerPool.customers)
            {
                if (customerProfile != null)
                {
                    GameObject newCustomer = Instantiate(customerPrefab, entrance.transform.position, Quaternion.identity);
                    CustomerController newCustomerController = newCustomer.GetComponent<CustomerController>();
                    newCustomerController.customerProfile = customerProfile;
                    yield return new WaitForSecondsRealtime(spawnWait);
                }
            }
            waveSpawning = false;
        }
    }

    //TODO Test this
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
        }
        if (claimedItem != null)
        {
            claimedItems.Add(item); 
        }
        return claimedItem;
    }
    //TODO test this, also not fully implemented
    public void UnclaimItem(ItemInstance item)
    {
        if (claimedItems.Contains(item))
        {
            claimedItems.Remove(item); 
        }
        unclaimedItems.Add(item);
        Debug.Log(unclaimedItems.Count);
    }
}

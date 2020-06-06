using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CustomerDirector : SerializedMonoBehaviour
{
    /// <summary>
    /// TODO Leaving this as a reminder for myself. What I'm going to do instead of what I've been trying, is just write the methods
    /// for claiming and unclaiming shelves from the dictionary, then go through and start to untangle the defunct code. Dunno why I didn't 
    /// think of this before...
    /// </summary>
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
    [SerializeField] private Dictionary<string, CustomerController> waitingCustomers = new Dictionary<string, CustomerController>();
    [SerializeField] private Dictionary<string, List<Shelf>> unclaimedShelves = new Dictionary<string, List<Shelf>>();
    [SerializeField] private Dictionary<string, List<Shelf>> claimedShelves = new Dictionary<string, List<Shelf>>();
    public bool waveSpawning = false;
    
    Slider progressSlider;

    public int WaveSize { get => waveSize;}
    public bool WaveFinished { get => waveFinished;}
    public Dictionary<string, CustomerController> WaitingCustomers { get => waitingCustomers; set => waitingCustomers = value; }
    public Dictionary<string, List<Shelf>> UnclaimedShelves { get => unclaimedShelves; set => unclaimedShelves = value; }
    public Dictionary<string, List<Shelf>> ClaimedShelves { get => claimedShelves; set => claimedShelves = value; }

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
        //If the wave has not already finished, and the wave has all been spawned, and all children have been destroyed, end wave
        if(!waveFinished && waveSize <= 0 && transform.childCount == 0)
        {
            waveFinished = true;
        }
        if(unclaimedShelves.Count > 0 && waitingCustomers.Count > 0)
        {
            List<string> waitingCustomerKeyList = new List<string>(waitingCustomers.Keys);
            string randomCustomerID = waitingCustomerKeyList[Random.Range(0, waitingCustomerKeyList.Count)];
            CustomerController randomCustomer = waitingCustomers[randomCustomerID];
            waitingCustomers.Remove(randomCustomerID);
            randomCustomer.TargetShelf = ClaimRandomShelf();
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnCustomers());
    }

    public IEnumerator SpawnCustomers()
    {
        waveSpawning = true;
        while (waveSpawning && waveSize >= 0)
        {
            //Get a random profile from the pool
            CustomerProfile customerProfile = customerPool.GetRandomProfile();
            Debug.Log("Spawning next customer: " + customerProfile.characterName);

            if (customerProfile != null)
            {
                GameObject newCustomer = Instantiate(customerPrefab, entrance.transform.position, Quaternion.identity,gameObject.transform);
                CustomerController newCustomerController = newCustomer.GetComponent<CustomerController>();
                newCustomerController.customerProfile = customerProfile;
                newCustomerController.levelExit = entrance;
                newCustomerController.shelfManager = shelfManager;
                waveSize--;
                yield return new WaitForSecondsRealtime(spawnWait);
            }
        }
        waveSpawning = false;
    }

    public void SpawnOneCustomer()
    {
        CustomerProfile customerProfile = customerPool.GetRandomProfile();
        GameObject newCustomer = Instantiate(customerPrefab, entrance.transform.position, Quaternion.identity, gameObject.transform);
        CustomerController newCustomerController = newCustomer.GetComponent<CustomerController>();
        newCustomerController.customerProfile = customerProfile;
        newCustomerController.levelExit = entrance;
        newCustomerController.shelfManager = shelfManager;
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
        if (!unclaimedItems.Contains(item))
        {
            unclaimedItems.Add(item); 
        }
    }

    public void UnclaimShelf(Shelf shelf)
    {
        if(shelf.HeldItem.item == null) { return; }
        var itemKey = shelf.HeldItem.GetItemID();
        if (unclaimedShelves.ContainsKey(itemKey))
        {
            unclaimedShelves[itemKey].Add(shelf);
        }
        else
        {
            List<Shelf> shelves = new List<Shelf>() { shelf };
            unclaimedShelves.Add(itemKey, shelves);
        }
    }

    public Shelf ClaimShelfByItem(ItemInstance item)
    {
        var itemKey = item.GetItemID();
        if (unclaimedShelves.ContainsKey(itemKey))
        {
            //if there are multiple shelves with this item, we can get a random shelf
            List<Shelf> shelves = unclaimedShelves[itemKey];
            int randomIndex = Random.Range(0, shelves.Count);
            Shelf randomShelf = shelves[randomIndex];
            //After we have the shelf we need to remove it and add it to claimed shelves.
            unclaimedShelves[itemKey].RemoveAt(randomIndex);
            AddToClaimedShelves(itemKey, randomShelf);
            return randomShelf;
        }
        return null;
    }

    private void AddToClaimedShelves(string itemKey, Shelf shelf)
    {
        if (claimedShelves.ContainsKey(itemKey))
        {
            claimedShelves[itemKey].Add(shelf);
        }
        else
        {
            List<Shelf> shelves = new List<Shelf>() { shelf };
            claimedShelves.Add(itemKey, shelves);
        }
    }

    public Shelf ClaimRandomShelf()
    {
        List<string> itemKeys = new List<string>(unclaimedShelves.Keys);
        string randomKey = itemKeys[Random.Range(0, itemKeys.Count)];
        List<Shelf> shelves = unclaimedShelves[randomKey];
        int randomIndex = Random.Range(0, shelves.Count);
        Shelf randomShelf = shelves[randomIndex];
        //After we have the shelf we need to remove it and add it to claimed shelves.
        unclaimedShelves[randomKey].RemoveAt(randomIndex);
        //If an item key now has no shelves we need to make sure to remove it.
        if(unclaimedShelves[randomKey].Count == 0)
        {
            unclaimedShelves.Remove(randomKey);
        }
        AddToClaimedShelves(randomKey, randomShelf);
        return randomShelf;
    }

    public void RemoveFromClaimedShelves(Shelf shelf)
    {
        string itemKey = shelf.HeldItem.GetItemID();
        int index = 0;
        var possibleShelves = claimedShelves[itemKey];
        foreach (Shelf possibleShelf in possibleShelves)
        {
            if (possibleShelf.name.Equals(shelf.name))
            {
                claimedShelves[itemKey].RemoveAt(index);
                if(claimedShelves[itemKey].Count == 0) { claimedShelves.Remove(itemKey); }
                return;
            }
            index++;
        }
    }
}

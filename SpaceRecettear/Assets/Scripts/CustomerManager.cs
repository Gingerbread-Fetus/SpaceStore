using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] CustomerPool customerPool;
    [SerializeField] GameObject customerPrefab;
    [SerializeField] GameObject entrance;
    [SerializeField] float spawnWait = 5f;

    public bool waveSpawning;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SpawnCustomers()
    {
        foreach (CustomerProfile customerProfile in customerPool.customers)
        {
            if (customerProfile != null)
            {
                waveSpawning = true;
                GameObject newCustomer = Instantiate(customerPrefab, entrance.transform.position, Quaternion.identity);
                CustomerController newCustomerController = newCustomer.GetComponent<CustomerController>();
                newCustomerController.customerProfile = customerProfile;
                yield return new WaitForSecondsRealtime(spawnWait); 
            }
        }
        waveSpawning = false;
    }
}

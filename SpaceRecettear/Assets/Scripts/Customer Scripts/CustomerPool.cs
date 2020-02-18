using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Customer Pool", menuName = "Customers/Pool")]
public class CustomerPool : ScriptableObject
{
    [SerializeField] public List<CustomerProfile> customers;

    public CustomerProfile GetRandomProfile()
    {
        CustomerProfile profile = null;

        if (customers.Count > 0)
        {
            profile = customers[Random.Range(0, customers.Count)];
        }

        return profile;
    }
}

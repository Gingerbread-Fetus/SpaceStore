using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Customer Pool", menuName = "Customers/Pool")]
public class CustomerPool : ScriptableObject
{
    [SerializeField] public List<CustomerProfile> customers;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    CustomerManager customerManager;
    public void Interact()
    {
        //Debug.Log("Door interacted with");
        //IEnumerator spawningCoroutine = customerManager.SpawnCustomers();
        //StartCoroutine(spawningCoroutine);
    }

    // Start is called before the first frame update
    void Start()
    {
        //customerManager = FindObjectOfType<CustomerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

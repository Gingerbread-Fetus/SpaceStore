using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Customer script handles the interactability, preferences,
/// and behaviors of the character it is attached to.
/// EG: A businessman character is more likely to come in and want pens. (I dunno)
/// </summary>
public class Customer : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Customer interacted with.");
    }

}

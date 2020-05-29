using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] RectTransform exitStorePromptPanel;
    CustomerDirector customerManager;
    public void Interact()
    {
        ShowPrompt();
    }

    private void ShowPrompt()
    {
        exitStorePromptPanel.gameObject.SetActive(true);
    }

    public void HidePanel()
    {
        exitStorePromptPanel.gameObject.SetActive(false);
    }

    public void ExitStore()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestButton : MonoBehaviour
{
    public QuestInfo quest;

    [HideInInspector]
    public Button buttonComponent;

    private void Start()
    {
        buttonComponent = GetComponent<Button>();
    }
}

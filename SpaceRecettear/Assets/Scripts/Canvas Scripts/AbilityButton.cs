using System;
using TMPro;
using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    TextMeshProUGUI textComponent;

    private Ability ability;

    public void SetAbility(Ability value)
    {
        ability = value;
        textComponent = GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = ability.aName;
    }

    
    void Start()
    {
        textComponent = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

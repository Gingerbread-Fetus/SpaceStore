using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string aName = "New Ability";
    public Sprite aSprite;
    public int aAbilityCost = 1;
    public AudioClip aSound;
    public GameObject aMinigamePrefab;
    public ItemInstance aItemInstance;

    [TextArea(10, 14)] [SerializeField]public string abilityDescription;
    public abstract GameObject Initialize();
    public abstract void TriggerAbility();
    public abstract void Cleanup();
}

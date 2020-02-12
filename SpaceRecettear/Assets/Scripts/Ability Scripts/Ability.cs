using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string aName = "New Ability";
    public Sprite aSprite;
    public int aCost = 1;
    public AudioClip aSound;
    public GameObject aMinigameCanvas;
    public ItemInstance aItemInstance;

    public abstract void Initialize(GameObject obj);
    public abstract void TriggerAbility();
}

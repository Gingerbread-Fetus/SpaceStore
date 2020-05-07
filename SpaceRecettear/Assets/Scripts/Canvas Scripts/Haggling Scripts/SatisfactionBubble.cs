using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SatisfactionBubble : MonoBehaviour
{
    Animator myAnimator;
    [SerializeField] Image faceImage;
    [SerializeField] Sprite[] satisfactionLevelSprites;

    int satisfactionLevel = 0;
    HagglingController hagglingController;

    [HideInInspector] public int satisfactionMax;

    void Start()
    {
        hagglingController = FindObjectOfType<HagglingController>();
        satisfactionMax = satisfactionLevelSprites.Length - 1;
        hagglingController.SatisfactionMax = satisfactionMax;
        satisfactionLevel = satisfactionMax;
        faceImage.sprite = satisfactionLevelSprites[satisfactionLevel];
    }

    void Update()
    {
        satisfactionLevel = hagglingController.SatisfactionLevel;
        if (satisfactionLevel > satisfactionMax)
        {
            satisfactionLevel = satisfactionMax;
        }
        else if(satisfactionLevel <= 0)
        {
            satisfactionLevel = 0;
        }
        faceImage.sprite = satisfactionLevelSprites[satisfactionLevel];
    }       
}

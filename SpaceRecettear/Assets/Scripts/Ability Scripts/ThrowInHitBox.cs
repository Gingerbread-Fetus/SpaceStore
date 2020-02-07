using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThrowInHitBox : MonoBehaviour
{
    [SerializeField] Animator spaceKeyAnimator;
    [SerializeField] Animator thrownItemAnimator;
    [SerializeField] SpritePathMovement spriteMover;

    bool isInHitbox;
    private bool hasTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !hasTriggered)
        {
            spriteMover.isMoving = false;
            hasTriggered = true;
            ThrowItem();
        }
    }

    private void ThrowItem()
    {
        if(isInHitbox && !spriteMover.isMoving)
        {
            thrownItemAnimator.SetBool("throwItem", true);
            thrownItemAnimator.SetBool("itemHit", true);
        }
        else
        {
            thrownItemAnimator.SetBool("throwItem", true);
            thrownItemAnimator.SetBool("itemHit", false);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        spaceKeyAnimator.SetBool("isInHitBox", true);
        isInHitbox = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        spaceKeyAnimator.SetBool("isInHitBox", false);
        isInHitbox = false;
    }
}

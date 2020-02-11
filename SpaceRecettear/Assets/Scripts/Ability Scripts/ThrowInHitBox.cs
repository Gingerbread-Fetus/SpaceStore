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
    [SerializeField] TextMeshProUGUI HitOrMissTextObject;


    bool isInHitbox;
    private bool hasTriggered = false;
    TextMeshProUGUI notificationText;

    // Start is called before the first frame update
    void Start()
    {
        HitOrMissTextObject.gameObject.SetActive(false);
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
            HitOrMissTextObject.gameObject.SetActive(true);
            spriteMover.success = true;
        }
        else
        {
            thrownItemAnimator.SetBool("throwItem", true);
            thrownItemAnimator.SetBool("itemHit", false);
            HitOrMissTextObject.gameObject.SetActive(true);
            HitOrMissTextObject.text = "Miss!";
            HitOrMissTextObject.color = Color.red;
            spriteMover.success = false;
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

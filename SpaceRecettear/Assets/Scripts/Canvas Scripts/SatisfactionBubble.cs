using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SatisfactionBubble : MonoBehaviour
{
    Animator myAnimator;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSatisfactionLevel(float newSatisfaction)
    {
        Debug.Log("Satisfaction Bubble satisfaction: " + newSatisfaction);
        myAnimator.SetFloat("SatisfactionLevel", newSatisfaction);
    }
}

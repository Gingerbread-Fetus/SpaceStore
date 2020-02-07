using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePathMovement : MonoBehaviour
{
    [SerializeField]GameObject waypointPathPrefab;
    [SerializeField]float moveSpeed;

    int waypointIndex = 0;
    RectTransform rectTransform;
    [HideInInspector]public bool isMoving = true;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = waypointPathPrefab.transform.GetChild(0).transform.position;
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            Move(); 
        }
    }

    public void pauseMovement()
    {
        isMoving = !isMoving;
    }

    private void Move()
    {
        if(waypointIndex <= waypointPathPrefab.transform.childCount - 1)
        {            
            var targetPosition = waypointPathPrefab.transform.GetChild(waypointIndex).transform.position;
            var movementThisFrame = moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPosition,
                movementThisFrame);
            if (transform.position == targetPosition)
            {
                waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

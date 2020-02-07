using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePathMovement : MonoBehaviour
{
    [SerializeField]List<GameObject> waypoints;
    int waypointIndex = 0;
    float moveSpeed;
    RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = waypoints[waypointIndex].transform.position;
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if(waypointIndex <= waypoints.Count - 1)
        {
            var targetPosition = waypoints[waypointIndex].transform.position;
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePathMovement : MonoBehaviour
{
    [SerializeField]float moveSpeed = 5f;

    List<Transform> waypoints = new List<Transform>();
    int waypointIndex = 0;
    RectTransform rectTransform;
    GameObject waypointPathPrefab;
    [HideInInspector]public bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
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

    public void PauseMovement()
    {
        isMoving = !isMoving;
    }

    private void Move()
    {

        if(waypointIndex <= waypoints.Count - 1)
        {            
            var targetPosition = waypoints[waypointIndex].position;
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

    public void SetWaypoints(GameObject waypointObject)
    {
        foreach(Transform waypoint in waypointObject.transform)
        {
            waypoints.Add(waypoint.transform);
        }
        transform.position = waypoints[0].position;
        isMoving = true;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePathMovement : MonoBehaviour
{
    [SerializeField]GameObject waypointPathPrefab;
    [SerializeField]float moveSpeed;

    List<Transform> waypoints = new List<Transform>();
    int waypointIndex = 0;
    RectTransform rectTransform;
    [HideInInspector]public bool isMoving = true;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        InstantiateWayPoints();
        GetWaypoints();
    }

    private void InstantiateWayPoints()
    {
        Instantiate(waypointPathPrefab, transform.parent.transform, true);
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

    private void GetWaypoints()
    {
        foreach(Transform child in waypointPathPrefab.transform)
        {
            waypoints.Add(child);
        }

        transform.position = waypoints[0].transform.position;
    }
}

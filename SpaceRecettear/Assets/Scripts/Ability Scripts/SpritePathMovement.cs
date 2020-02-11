using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpritePathMovement : MonoBehaviour
{
    [SerializeField]float moveSpeed = 5f;
    [SerializeField]GameObject activeCustomer;
    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool success;

    List<Transform> waypoints = new List<Transform>();
    int waypointIndex = 0;
    RectTransform movingRectTransform;
    GameObject waypointPathPrefab;
    Image imageRenderer;

    private bool hasTriggered;

    // Start is called before the first frame update
    void Start()
    {
        movingRectTransform = activeCustomer.GetComponent<RectTransform>();
        imageRenderer = activeCustomer.GetComponent<Image>();
        hasTriggered = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            Move(); 
        }

        if (success)
        {
            SetResults();
        }
        else if(!success && !isMoving)
        {
            Invoke("DestroyGameObject", 3.0f);
        }
    }

    private void Move()
    {

        if(waypointIndex <= waypoints.Count - 1)
        {            
            var targetPosition = waypoints[waypointIndex].position;
            var movementThisFrame = moveSpeed * Time.deltaTime;
            movingRectTransform.position = Vector2.MoveTowards(
                movingRectTransform.position,
                targetPosition,
                movementThisFrame);
            if (movingRectTransform.position == targetPosition)
            {
                waypointIndex++;
            }
        }
        else
        {
            isMoving = false;
        }
    }

    private void SetResults()
    {
        //TODO: Implement the logic handling for the ability here.
        Invoke("DestroyGameObject", 3.0f);
    }

    private void DestroyGameObject()
    {
        Destroy(gameObject);
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

    public void ChangeSprite(Sprite newSprite)
    {
        imageRenderer.sprite = newSprite;
    }

    public void PauseMovement()
    {
        isMoving = !isMoving;
    }
}

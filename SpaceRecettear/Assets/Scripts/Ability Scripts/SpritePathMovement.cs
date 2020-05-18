using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpritePathMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] GameObject activeCustomer;
    [SerializeField] Image itemImage;
    [SerializeField] AbilityInventoryHandler inventoryHandler;
    [HideInInspector] public bool isMoving = false;
    [HideInInspector] public bool success;

    ItemInstance selectedItem;
    List<Transform> waypoints = new List<Transform>();
    int waypointIndex = 0;
    RectTransform movingRectTransform;
    GameObject waypointPathPrefab;
    Image customerImageRenderer;


    private bool hasTriggered;

    // Start is called before the first frame update
    void Start()
    {
        movingRectTransform = activeCustomer.GetComponent<RectTransform>();
        customerImageRenderer = activeCustomer.GetComponent<Image>();
        hasTriggered = false;
    }
    
    void Update()
    {
        if (isMoving)
        {
            Move(); 
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
            float distance = Vector2.Distance(movingRectTransform.position, targetPosition);
            if (distance < .0001f)
            {
                waypointIndex++;
            }
        }
        else
        {
            isMoving = false;
        }
    }

    public void SetResults()
    {
        if (success)
        {
            HagglingController hagglingController = FindObjectOfType<HagglingController>();
            hagglingController.AddItemToTransaction(selectedItem);
            Debug.Log("Success");
        }
        else
        {
            inventoryHandler.ReturnItem();
        }
        Invoke("ExitMiniGame", 3.0f);
    }

    private void ExitMiniGame()
    {
        gameObject.SetActive(false);
    }

    public void SetWaypoints(GameObject waypointObject)
    {
        foreach(Transform waypoint in waypointObject.transform)
        {
            waypoints.Add(waypoint.transform);
        }
        transform.position = waypoints[0].position;
    }

    public void ChangeCustomer(Sprite newSprite)
    {
        customerImageRenderer.sprite = newSprite;
    }

    public void ChangeItem(ItemInstance newItem)
    {
        selectedItem = newItem;
        itemImage.sprite = newItem.item.itemIcon;
    }

    public void PauseMovement()
    {
        isMoving = !isMoving;
    }

    public void StartMiniGame()
    {
        ChangeItem(inventoryHandler.selectedItem);
        isMoving = true;
        hasTriggered = true;
    }
}

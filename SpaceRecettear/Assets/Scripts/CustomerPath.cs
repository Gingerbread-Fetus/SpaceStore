using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerPath : MonoBehaviour
{
    [SerializeField] Vector3 startingPosition;//Serializing for debugging
    [SerializeField] Vector3 endingPosition;//Serializing for debugging
    
    Cell startingCell;
    LayerMask layerMask;
    SortedList<Cell, Vector3> openSet;
    SortedList<Cell, Vector3> closedSet;

    private void Start()
    {
        layerMask = LayerMask.GetMask("Interactable", "Customers", "Player", "Walls");
        startingPosition = transform.position;
    }

    private void Update()
    {
        
    }

    public void SetEndingPosition(Vector3 newEndingPos)
    {
        endingPosition = newEndingPos;
    }

    public void FindPathAStar()
    {
        //TODO: This is where you were. Implement pathfinding.
    }
}

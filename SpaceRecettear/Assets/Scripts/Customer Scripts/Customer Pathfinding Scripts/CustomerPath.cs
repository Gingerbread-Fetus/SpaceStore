using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerPath : MonoBehaviour
{
    [SerializeField] bool drawDebugging = false;
    [SerializeField] Vector3 startingPosition;//Serializing for debugging
    [SerializeField] Vector3 endingPosition;//Serializing for debugging
    
    Cell startingCell;
    LayerMask layerMask;
    MinHeap<Cell> frontier;
    Dictionary<Cell, double> costSoFar;//This is the set of visited nodes.
    List<Cell> solutionList;

    private Cell goalCell;

    public List<Cell> GetPath() => solutionList;

    private void Start()
    {
        layerMask = LayerMask.GetMask("Interactable", "Walls");
        solutionList = new List<Cell>();

         // A* Search Algorithm
        frontier = new MinHeap<Cell>();
        costSoFar = new Dictionary<Cell,double>();
        startingPosition = gameObject.transform.position;
        SetEndPoints(startingPosition, endingPosition);
        

        //Cell startingCell = new Cell(startingPosition, endingPosition);
        //frontier.Add(startingCell);
        //costSoFar[startingCell] = 0;
    }

    private void Update()
    {
        
    }

    private List<Cell> ConstructPath()
    {
        
        Cell parentCell = goalCell;

        while (parentCell != null)
        {
            solutionList.Add(parentCell);
            parentCell = parentCell.parent;
        }

        solutionList.Reverse();
        if (drawDebugging)
        {
            printSolution(); 
        }
        return solutionList;
    }
    
    private string printSolution()
    {
        string sb = "";
        foreach(Cell cell in solutionList)
        {
            Debug.DrawRay(cell.Position, Vector3.up * .5f, Color.blue, 3.0f);
            sb += cell.ToString() + "\n";
        }
        return sb;
    }

    public void SetEndPoints(Vector3 newStartPos ,Vector3 newEndingPos)
    {
        startingPosition = newStartPos;
        endingPosition = newEndingPos;
        goalCell = new Cell(endingPosition, endingPosition);
        frontier = new MinHeap<Cell>();
        costSoFar = new Dictionary<Cell, double>();
        Cell startingCell = new Cell(startingPosition, endingPosition);
        frontier.Add(startingCell);
        costSoFar[startingCell] = 0;
    }

    /// <summary>
    /// </summary>
    public void FindPathAStar()
    {
        while(frontier.Count > 0)
        {
            Cell currentCell = frontier.RemoveMin();
            if (currentCell.CompareTo(goalCell) == 0)
            {
                goalCell = currentCell;
                //Debug.Log("Goal Cell found: " + goalCell.ToString());
                ConstructPath();
                //Debug.Break();//This is great for debugging purposes
                break;
            }
            //for neighbors of current:
            foreach (Cell neighbor in GetNeighbors(currentCell))
            {
                double newCost = costSoFar[currentCell] + neighbor.cellMovementCost;

                if(!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                {
                    costSoFar[neighbor] = newCost;
                    frontier.Add(neighbor);
                }
            }
        }//end(while loop)
    }

    /// <summary>
    /// Gets the neighboring cells that are valid for pathfinding.
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    private List<Cell> GetNeighbors(Cell cell)
    {
        List<Cell> neighbors = new List<Cell>();

        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                Vector3 tmp = new Vector3(x, y, 0);
                Vector3 nextPos = tmp + cell.Position;
                if(x==0 && y == 0) { continue; }//skip over the 'center' cell.
                Cell nextSuccessor = new Cell(cell, nextPos, endingPosition);////for neighbors of current: cost = g(current) + movementcost(current, neighbor)
                RaycastHit2D hit = Physics2D.Raycast(cell.Position, (nextSuccessor.Position - cell.Position).normalized, 1f, layerMask);

                if (hit)//If fraction <= 0 then the collision came from inside the collider.
                {
                    //Debug.Log(hit + ": hit detected by raycast on " + hit.collider.gameObject.name);
                    continue;
                }
                else
                {
                    if (drawDebugging){ Debug.DrawRay(nextSuccessor.Position, Vector3.right * .5f, Color.yellow, 3.0f);}
                    nextSuccessor.parent = cell;
                    neighbors.Add(nextSuccessor);
                }
            }
        }
        return neighbors;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerPath : MonoBehaviour
{
    [SerializeField] bool drawDebugging;
    [SerializeField] Vector3 startingPosition;//Serializing for debugging
    [SerializeField] Vector3 endingPosition;//Serializing for debugging
    
    Cell startingCell;
    LayerMask layerMask;
    SortedList<Cell,Cell> openList;
    SortedList<Cell, Cell> closedList;
    List<Cell> solutionList;

    private void Start()
    {
        layerMask = LayerMask.GetMask("Interactable", "Customers",  "Walls");
        solutionList = new List<Cell>();
    }

    private void Update()
    {
        
    }

    public List<Cell> GetPath() => solutionList;

    public void SetEndPoints(Vector3 newStartPos ,Vector3 newEndingPos)
    {
        startingPosition = newStartPos;
        endingPosition = newEndingPos;
    }

    public void FindPathAStar()
    {
        // A* Search Algorithm
        openList = new SortedList<Cell, Cell>();
        closedList = new SortedList<Cell, Cell>();

        //put the starting node on the open
        //list(you can leave its f at zero)
        Cell startingCell = new Cell(0, startingPosition, endingPosition);
        openList.Add(startingCell, startingCell);
        //3.  while the open list is not empty
        while (openList.Count > 0)
        {
            //Find the node with the lowest cost off the OPEN list.
            Cell currentCell = openList.Values[0];
            openList.Remove(currentCell);

            //If the cell is the solution, populate the solution list.
            if (currentCell.Position.Equals(endingPosition))
            {
                while(currentCell != null)
                {
                    solutionList.Add(currentCell);
                    currentCell = currentCell.parent;
                }
                solutionList.Reverse();
                break;
            }

            //generate cells successors and set their parent to the cell.
            List<Cell> successors = GetSuccessors(currentCell);
            //    d) for each successor
            foreach (Cell successor in successors)
            {
                //First check if successor is in the open list
                Cell openCell = null;
                if (openList.ContainsKey(successor))
                {
                    openCell = openList[successor];
                }
                //If it is, check the finalWeight, and discard it if it is higher.
                if(openCell != null && successor.FinalWeight > openCell.FinalWeight)
                {
                    continue;
                }
                else if(openCell != null)
                {
                    //Remove the old successor from the open list
                    openList.Remove(openCell);
                }

                //Then do the same thing on the CLOSED list
                Cell closedCell = null;
                if (closedList.ContainsKey(successor))
                {
                    closedCell = closedList[successor];
                }
                if(closedCell != null && successor.FinalWeight > closedCell.FinalWeight)
                {
                    continue;
                }
                else if(closedCell != null)
                {
                    //Remove the old successor from the closed list
                    closedList.Remove(closedCell);
                }

                //Add the current successor to the open list
                openList.Add(successor,successor);
            }//end(for loop)

            //Add the currentCell to the closed list.
            Debug.Log(currentCell.Position);
            closedList.Add(currentCell,currentCell);//TODO: There seems to be a problem with this line and items with the same key being added. Giving a ArgumentException.

        }//end(while loop)
    }

    private List<Cell> GetSuccessors(Cell cell)
    {
        List<Cell> successors = new List<Cell>();

        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                Vector3 tmp = new Vector3(x, y, 0);
                Vector3 nextPos = tmp + cell.Position;
                Cell nextSuccessor = new Cell(cell.MovementCost + 1, nextPos, endingPosition);
                Debug.Log(cell.Position + " to " + nextSuccessor.Position);
                Debug.DrawRay(cell.Position, nextSuccessor.Position, Color.green);
                RaycastHit2D hit = Physics2D.Raycast(cell.Position, nextSuccessor.Position, 1f, layerMask);
                Debug.Log(hit + " at " + hit.collider.gameObject.name);
                if (!hit)//TODO: Doesn't work. Only seems to be hitting the door. Doesn't seem to be raycasting in the right direction.
                {
                    nextSuccessor.parent = cell;
                    successors.Add(nextSuccessor); 
                }
            }
        }

        return successors;
    }
}

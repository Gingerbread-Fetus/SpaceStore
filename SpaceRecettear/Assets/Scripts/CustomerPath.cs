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
            Debug.Log(openList.Remove(currentCell));

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
                //If it is, check the finalWeight, and discard it if it is higher.
                Cell openCell = null;
                if (openList.ContainsKey(successor))
                {
                    openCell = openList[successor];
                }
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
            ///TODO: There seems to be a problem with this line and items with
            ///the same key being added. Giving a ArgumentException.
            bool test = closedList.ContainsKey(currentCell);
            Debug.Log(test);
            closedList.Add(currentCell,currentCell);

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
                if(x==0 && y == 0) { continue; }//skip over the 'center' cell.
                Cell nextSuccessor = new Cell(cell.MovementCost + 1, nextPos, endingPosition);
                Debug.Log(cell.Position + " to " + nextSuccessor.Position);
                RaycastHit2D hit = Physics2D.Raycast(cell.Position, (nextSuccessor.Position - cell.Position).normalized, 1f, layerMask);

                if (hit)//If fraction <= 0 then the collision came from inside the collider.
                {
                    Debug.Log(hit + ": hit detected by raycast on " + hit.collider.gameObject.name);
                    //TODO: Handle edge cases of collision here. EG: if the collider hit is this collider, we still want to add it to successors.
                    if (hit.collider.gameObject.Equals(gameObject))//This doesn't work.
                    {
                        Debug.Log("Self containing cell was hit");
                        successors.Add(nextSuccessor);
                    }
                    continue;
                }
                else
                {
                    nextSuccessor.parent = cell;
                    successors.Add(nextSuccessor);
                }
            }
        }

        return successors;
    }
}

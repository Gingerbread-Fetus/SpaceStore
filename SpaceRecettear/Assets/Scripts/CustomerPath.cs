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
    SortedList<Cell,Cell> openSet;
    SortedList<Cell, Cell> closedSet;

    private void Start()
    {
        layerMask = LayerMask.GetMask("Interactable", "Customers", "Player", "Walls");
    }

    private void Update()
    {
        
    }

    public IList<Cell> GetPath() => closedSet.Values;

    public void SetEndPoints(Vector3 newStartPos ,Vector3 newEndingPos)
    {
        startingPosition = newStartPos;
        endingPosition = newEndingPos;
    }

    public void FindPathAStar()
    {
        //TODO: This is where you were. Implement pathfinding, cells aren't being added to the open set.
        // A* Search Algorithm
        //1.Initialize the open list
        openSet = new SortedList<Cell, Cell>();
        //2.Initialize the closed list
        closedSet = new SortedList<Cell, Cell>();


        //put the starting node on the open
        //list(you can leave its f at zero)
        Cell startingCell = new Cell(0, startingPosition, endingPosition);
        openSet.Add(startingCell, startingCell);
        //3.  while the open list is not empty
        while (openSet.Count > 0)
        {
            //    a) find the node with the least f on  the open list, call it currentCell
            Cell currentCell = openSet.Values[0];
            //    b) pop q off the open list
            openSet.Remove(currentCell);

            //    c) generate q's 8 successors and set their 
            //       parents to q
            List<Cell> successors = GetSuccessors(currentCell);
            //    d) for each successor
            foreach (Cell successor in successors)
            {
                //i) if successor is the goal, stop search
                if (successor.Position.Equals(endingPosition))
                {
                    closedSet.Add(successor, successor);
                    return;
                }
                //ii) if a node with the same position as successor is in the OPEN list...
                Cell cell;
                if (openSet.TryGetValue(successor, out cell))//TODO: may be a problem here, make sure to test it.
                {
                    //which has a lower finalWeight than successor....
                    if (cell.FinalWeight < successor.FinalWeight)
                    {
                        continue;//skip this successor 
                    }
                }
                //iii) if a node with the same position as successor is in the CLOSED list... 
                if (closedSet.TryGetValue(successor, out cell))
                {
                    //which has a lower f than successor...
                    if (cell.FinalWeight < successor.FinalWeight)
                    {
                        continue;//...skip this successor 
                    }
                    else//otherwise, add  the node to the open list
                    {
                        openSet.Add(successor, successor);
                    }
                }

            }//end(for loop)

            //e) push q on the closed list 
            closedSet.Add(currentCell,currentCell);
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
                nextSuccessor.parent = cell;
                successors.Add(nextSuccessor);
            }
        }

        return successors;
    }

    private bool CellIsValid()
    {
        throw new NotImplementedException();
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Cell : IComparable
{
    private int D => 1;//Minimum cost for moving between spaces
    private int D2 => 1;//Minimum cost for moving diagonally.
    public double cellMovementCost = 1f;

    public Cell parent;
    public Vector3 Position { get; }//Position in game world of the cell.
    public Vector3 Goal { get; private set; }

    //f = g + h
    //f want the lowest value to sort them by
    //g represents the exact cost of the path from the starting point to any vertex n
    //h represents the heuristic estimated cost
    public double Heuristic { get; set; }//Distance from cell to goal, this is h.
    public double MovementCost { get; set; }//Distance from start to this cell. This is g.
    public double FinalWeight { get; set; }//The weight used to prioritize the path, A* prioritizes lowest finalWeight. This is f

    /// <summary>
    /// This constructor will be used to make the original node, so it won't have  parent.
    /// </summary>
    /// <param name="position">Position of cell.</param>
    /// <param name="goalPosition">The cell we are trying to go to</param>
    public Cell(Vector3 position, Vector3 goalPosition)
    {
        //distance from goal
        this.parent = null;
        this.MovementCost = 0;
        this.Position = position;
        this.Goal = goalPosition;
        this.Heuristic = GetHeuristic(this);
        this.FinalWeight = MovementCost + Heuristic;
    }

    /// <summary>
    /// </summary>
    /// <param name="movementCost"></param>
    /// <param name="position"></param>
    /// <param name="goalPosition"></param>
    public Cell(Cell parent, Vector3 position, Vector3 goalPosition)
    {
        //distance from goal
        this.parent = parent;
        this.MovementCost = parent.MovementCost + cellMovementCost;
        this.Position = position;
        this.Goal = goalPosition;
        this.Heuristic = GetHeuristic(this);
        this.FinalWeight = MovementCost + Heuristic;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public int CompareTo(object obj)
    {
        if (obj == null) { return 1; }

        if (obj is Cell otherCell)
        {
            int distanceCheck = this.Heuristic.CompareTo(otherCell.Heuristic);
            if(distanceCheck == 0)//if they have the same weight we have to check the postion.
            {
                if (this.Equals(otherCell))
                {
                    return 0;//Return 0 if they are indeed the same object
                }
                else
                {
                    return 1;//Otherwise we'll sort it after the cell.
                }
            }
            else//Otherwise we can just return weight check
            {
                return distanceCheck;
            }
        }
        else
        {
            throw new ArgumentException("Object is not a Cell");
        }
    }

    public override bool Equals(object obj)
    {
        if (obj == null) { return false; }
        if (obj is Cell otherCell) { return otherCell.Position.Equals(this.Position); }
        return false;
    }

    public override int GetHashCode()
    {
        return Position.GetHashCode();
    }

    public override string ToString()
    {
        return "(Position: " + Position.ToString() + ", Weight: " + FinalWeight.ToString() + ")";
    }

    /// <summary>
    /// Returns the manhattan distance for the given cell.
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    private double GetHeuristic(Cell cell)
    {
        Vector3 pos = cell.Position;
        Vector3 goal = cell.Goal;
        double dx = Math.Abs(pos.x - goal.x);
        double dy = Math.Abs(pos.y - goal.y);
        return D * (dx + dy) + (D2 - 2 * D) * Math.Min(dx, dy);
    }
}

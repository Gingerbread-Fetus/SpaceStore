using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Cell : IComparable
{
    Vector3 position;//Position in game world of the cell.

    //f = g + h
    double Heuristic { get; }//Distance from cell to goal, this is h.
    double MovementCost { get; }//The cost of moving from starting point to cell on the grid. This is g.
    double FinalWeight { get; }//The weight used to prioritize the path, A* prioritizes lowest finalWeight. This is f

    

    public Cell(double heuristic, double movementCost, Vector3 position)
    {
        this.MovementCost = movementCost;
        this.Heuristic = heuristic;
        this.FinalWeight = heuristic + movementCost;
        this.position = position;
    }

    /// <summary>
    /// TODO, this really needs testing.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public int CompareTo(object obj)
    {
        if (obj == null) { return 1; }

        if (obj is Cell otherCell)//Especially this part.
        {
            return this.FinalWeight.CompareTo(otherCell.FinalWeight);
        }
        else
        {
            throw new ArgumentException("Object is not a Cell");
        }
    }

    public override bool Equals(object obj)
    {
        if (obj == null) { return false; }
        if (obj is Cell otherCell) { return otherCell.position.Equals(this.position); }
        return false;
    }

    public override int GetHashCode()
    {
        var hashCode = -615065587;
        hashCode = hashCode * -1521134295 + Heuristic.GetHashCode();
        hashCode = hashCode * -1521134295 + MovementCost.GetHashCode();
        hashCode = hashCode * -1521134295 + FinalWeight.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<Vector3>.Default.GetHashCode(position);
        return hashCode;
    }
}

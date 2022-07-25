//DESC: Pathfinding node grid object (old version)
//NAME: Gerard Guix�
//DATE: 06/2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNodeOld
{

    private Grid<PathNodeOld> grid;
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public bool isWalkable;
    public PathNodeOld cameFromNode;

    public PathNodeOld(Grid<PathNodeOld> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
        grid.TriggerGridObjectChanged(x, y);
    }

    public override string ToString()
    {
        return x + "," + y;
    }

}

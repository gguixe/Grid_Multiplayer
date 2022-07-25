//DESC: Pathfinding system for movement of units
//NAME: Gerard Guixé
//DATE: 06/2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridPathfindingSystem;

public class Pathfinding
{

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public static Pathfinding Instance { get; private set; }
    private Grid<PathNodeOld> grid;
    private List<PathNodeOld> openList;
    private List<PathNodeOld> closedList;

    public Pathfinding(int width, int height)
    {
        Instance = this;
        grid = new Grid<PathNodeOld>(width, height, 10f, Vector3.zero, (Grid<PathNodeOld> g, int x, int y) => new PathNodeOld(g, x, y));
    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition) //Convert Vector3 input to PathNodeOld and returns Vector3
    {
        grid.GetXY(startWorldPosition, out int startX, out int startY);
        grid.GetXY(endWorldPosition, out int endX, out int endY);

        List<PathNodeOld> path = FindPath(startX, startY, endX, endY);
        if(path == null)
        {
            return null;
        } else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (PathNodeOld PathNodeOld in path)
            {
                vectorPath.Add(new Vector3(PathNodeOld.x, PathNodeOld.y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f);
            }
            return vectorPath;
        }
    }

    public List<PathNodeOld> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNodeOld startNode = grid.GetGridObject(startX, startY);
        PathNodeOld endNode = grid.GetGridObject(endX, endY);
		
		if (startNode == null || endNode == null) 
		{
			// Invalid Path
			return null;
        }

        openList = new List<PathNodeOld> { startNode };
        closedList = new List<PathNodeOld>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNodeOld PathNodeOld = grid.GetGridObject(x, y);
                PathNodeOld.gCost = 99999999;
                PathNodeOld.CalculateFCost();
                PathNodeOld.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNodeOld currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                return CalculatePath(endNode); //Reached final node
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNodeOld neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;

                if (!neighbourNode.isWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        //Out of nodes on open list
        return null;
    }

    public PathNodeOld GetNode(int x, int y)
    {
        return grid.GetGridObject(x, y);
    }

    public Grid<PathNodeOld> GetGrid()
    {
        return grid;
    }

    private List<PathNodeOld> GetNeighbourList(PathNodeOld currentNode)
    {
        List<PathNodeOld> neightbourList = new List<PathNodeOld>();

        if (currentNode.x - 1 >= 0)
        {
            //Left
            neightbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            //Left Down
            if(currentNode.y -1 >= 0) neightbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            //Left Up
            if (currentNode.y + 1 < grid.GetHeight()) neightbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
        }
        if (currentNode.x + 1 < grid.GetWidth())
        {
            //Right
            neightbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            //Right Down
            if (currentNode.y - 1 >= 0) neightbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            //Right Up
            if (currentNode.y + 1 < grid.GetHeight()) neightbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
        }

        //Down
        if (currentNode.y - 1 >= 0) neightbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        //Up
        if (currentNode.y + 1 < grid.GetHeight()) neightbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neightbourList;
    }

    private List<PathNodeOld> CalculatePath(PathNodeOld endNode)
    {
        List<PathNodeOld> path = new List<PathNodeOld>();
        path.Add(endNode);
        PathNodeOld currentNode = endNode;

        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }

        path.Reverse();

        //SHOW CALCULATED PATH AS 2D LINE
        bool showDebug = true;
        if (path != null && showDebug)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, new Vector3(path[i + 1].x, path[i + 1].y) * 10f + Vector3.one * 5f, Color.green, 5f);
            }
        }

        return path;
    }

    private int CalculateDistanceCost(PathNodeOld a, PathNodeOld b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNodeOld GetLowestFCostNode(List<PathNodeOld> PathNodeOldList)
    {
        PathNodeOld lowestFCostNode = PathNodeOldList[0];
        for(int i = 1; i < PathNodeOldList.Count; i++)
        {
            if(PathNodeOldList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = PathNodeOldList[i];
            }
        }

        return lowestFCostNode;
    }

}

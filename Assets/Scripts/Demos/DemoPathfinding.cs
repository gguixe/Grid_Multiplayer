//DESC: Demo for pathfinding movement in grid
//NAME: Gerard Guixé
//DATE: 06/2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using CodeMonkey;
//TUTORIAL HELP: https://www.youtube.com/watch?v=waEsGu--9P8&t=111s


public class DemoPathfinding : MonoBehaviour
{
    [SerializeField] private UnitMovement unitMovement;

    private Pathfinding pathfinding;
    private void Start()
    {
        pathfinding = new Pathfinding(5, 5);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            unitMovement.SetTargetPosition(mouseWorldPosition);

            //List<PathNode> path = pathfinding.FindPath(0, 0, x, y);

            //bool showDebug = false;
            //if (path != null && showDebug)
            //{
            //    for(int i=0; i<path.Count - 1; i++)
            //    {
            //        Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, new Vector3(path[i + 1].x, path[i + 1].y) * 10f + Vector3.one * 5f, Color.green, 5f);
            //    }
            //}

        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            pathfinding.GetNode(x, y).SetIsWalkable(!pathfinding.GetNode(x, y).isWalkable);
        }
    }
}





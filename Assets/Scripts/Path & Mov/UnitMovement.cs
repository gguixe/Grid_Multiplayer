//DESC: Unit movement using basic pathfinding (demo version)
//NAME: Gerard Guixé
//DATE: 06/2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
//using V_AnimationSystem;

public class UnitMovement : MonoBehaviour
{

    private const float speed = 40f;

    private int currentPathIndex;
    private List<Vector3> pathVectorList;

    private void Update()
    {
        HandleMovement();

        if (Input.GetMouseButtonDown(0))
        {
            SetTargetPosition(UtilsClass.GetMouseWorldPosition());
        }
    }

    private void HandleMovement()
    {
        if(pathVectorList != null)
        {
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            if(Vector3.Distance(transform.position, targetPosition) > 1f)
            {
                Vector3 moveDir = (targetPosition - transform.position).normalized;

                float distanceBefore = Vector3.Distance(transform.position, targetPosition);
                //SET MOVE VECTOR(SET DIRECTION WITH MOVE DIR HERE) NOW IT'S STILL A SQUARE
                transform.position = transform.position + moveDir * speed * Time.deltaTime;
            }
            else
            {
                currentPathIndex++;
                if(currentPathIndex >= pathVectorList.Count)
                {
                    StopMoving();
                    //SET DIRECTIONAL VECTOR WHEN STOPING TOO
                }
            }
        }
        else
        {
            //SET DIRECTIONAL VECTOR WHEN STOPING TOO
        }
    }

    private void StopMoving()
    {
        pathVectorList = null;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;
        pathVectorList = Pathfinding.Instance.FindPath(GetPosition(), targetPosition);
        //List<PathNode> pathNodeList = GridPathfindingSystem.GridPathfinding.Instance.GetPath(GetPosition(), targetPosition);

        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }
}

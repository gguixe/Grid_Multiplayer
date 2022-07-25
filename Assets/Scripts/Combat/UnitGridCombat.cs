using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;


public class UnitGridCombat : MonoBehaviour
{
    [SerializeField] private Team team;
    private State state;
    private MovePositionPathfinding movePosition;

    public enum Team
    {
        Blue,
        Red,
        Green,
        Purple
    }

    private enum State
    {
        Normal,
        Moving,
        Attacking
    }

    private void Awake()
    {
        state = State.Normal;
        movePosition = GetComponent<MovePositionPathfinding>();
    }

    private void Update()
    {
        switch (state)
        {
            case State.Normal:
                break;
            case State.Moving:
                break;
            case State.Attacking:
                break;
        }
    }
    public void MoveTo(Vector3 targetPosition, Action onReachedPosition) //Move unit towards position (using pathfinding algorithm "MovePositionPathfinding")
    {
        state = State.Moving;
        movePosition.SetMovePosition(targetPosition + new Vector3(1, 1), () => {
            state = State.Normal;
            onReachedPosition();
        });
    }

    //GET DATA FROM UNIT
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Team GetTeam()
    {
        return team;
    }

    public bool IsEnemy(UnitGridCombat unitGridCombat)
    {
        return unitGridCombat.GetTeam() != team;
    }

}

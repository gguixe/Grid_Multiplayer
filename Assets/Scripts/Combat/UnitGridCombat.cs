using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;


public class UnitGridCombat : MonoBehaviour
{
    [SerializeField] private Team team;
    [SerializeField] private int maxMoveDistance = 3;
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

    public bool CanAttackUnit(UnitGridCombat unitGridCombat)
    {
        return Vector3.Distance(GetPosition(), unitGridCombat.GetPosition()) < 50f; //Attack distance is 50f right now
    }

    public void AttackUnit (UnitGridCombat unitGridCombat, Action onAttackComplete)
    {
        state = State.Attacking;
        //GameHandler_GridCombatSystem.Instance.ScreenShake();
        //Debug.Log("ATACKED ENEMY " + unitGridCombat);
        state = State.Normal; onAttackComplete();
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

    public int GetmaxMoveDistance()
    {
        return maxMoveDistance;
    }

    public bool IsEnemy(UnitGridCombat unitGridCombat)
    {
        return unitGridCombat.GetTeam() != team;
    }

}

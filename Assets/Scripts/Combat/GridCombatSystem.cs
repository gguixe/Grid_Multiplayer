//DESC: Grid combat system (basic)
//NAME: Gerard Guixé
//DATE: 06/2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridCombatSystem : MonoBehaviour
{
    [SerializeField] private UnitGridCombat unitGridCombat;

    private State state;

    private enum State
    {
        Normal,
        Waiting
    }

    private void Awake()
    {
        state = State.Normal;
    }

    private void Start()
    {
        UpdateValidMovePosition();
    }

    private void UpdateValidMovePosition()
    {
        Grid<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid(); //We get combat grid
        grid.GetXY(unitGridCombat.GetPosition(), out int unitX, out int unitY); //We get unit position
        GridPathfindingSystem.GridPathfinding gridPathfinding = GameHandler_GridCombatSystem.Instance.gridPathfinding;

        //Set entire tilemap to invisible
        GameHandler_GridCombatSystem.Instance.GetMovementTilemap().SetAllTilemapSprite(MovementTilemap.TilemapObject.TilemapSprite.None);

        //Reset all grid valid move positions (all false until checked again)
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for(int y = 0; y < grid.GetHeight(); y++)
            {
                grid.GetGridObject(x, y).SetIsValidMovePosition(false);
            }
        }

        int maxMoveDistance = 3; //We want to limit the distance each unit can move
        for (int x=unitX - maxMoveDistance; x < unitX + maxMoveDistance; x++) 
        {
            for (int y = unitY - maxMoveDistance; y < unitY + maxMoveDistance; y++)
            {
                //Debug.Log(gridPathfinding.IsWalkable(x, y));
                //THIS IS ONLY WORKING WHEN WE ARE NOT ON THE EDGE OF THE GRID (BUG), IF THE UNIT HAS A BOX COLLIDER IT ALSO DOESN'T WORK
                if (gridPathfinding.IsWalkable(x, y)) //gridPathfinding.IsWalkable(x, y)
                {
                    //Position is walkable
                    if (gridPathfinding.HasPath(unitX, unitY, x , y))
                    {
                        //There is path
                        if (gridPathfinding.GetPath(unitX, unitY, x, y).Count <= maxMoveDistance)
                        {
                            //Path within move distance
                            //Set Tilemap to move
                            GameHandler_GridCombatSystem.Instance.GetMovementTilemap().SetTilemapSprite(x, y, MovementTilemap.TilemapObject.TilemapSprite.Move);
                            grid.GetGridObject(x, y).SetIsValidMovePosition(true);
                        } else
                        {
                            //Path outside move distance
                        }
                    }
                    else
                    {
                        //No valid path
                    }
                }
                else
                {
                    //Position is not walkable
                }
            }
        }
    }

    private void Update()
    {
        switch (state)
        {
            case State.Normal:
                if (Input.GetMouseButtonDown(0))
                {
                    //If the grid position is set as valid we allow movement (we get the grid cell on mouse position and check if cell is valid)
                    if (GameHandler_GridCombatSystem.Instance.GetGrid().GetGridObject(UtilsClass.GetMouseWorldPosition()).GetIsValidMovePosition())
                    {
                        //Valid Move Position
                        state = State.Waiting;
                        unitGridCombat.MoveTo(UtilsClass.GetMouseWorldPosition(), () => { state = State.Normal; UpdateValidMovePosition(); }); //With 3D game we should change this function //We use delegate on function to update state and positions
                    }
                }
                break;
            case State.Waiting:
                break;
        }
    }

    public class GridObject
    {

        private Grid<GridObject> grid;
        private int x;
        private int y;
        private bool isValidMovePosition;
        private UnitGridCombat unitGridCombat;

        public GridObject(Grid<GridObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetIsValidMovePosition(bool set)
        {
            isValidMovePosition = set;
        }

        public bool GetIsValidMovePosition()
        {
            return isValidMovePosition;
        }

        public void SetUnitGridCombat(UnitGridCombat unitGridCombat)
        {
            this.unitGridCombat = unitGridCombat;
        }

        public void ClearUnitGridCombat()
        {
            SetUnitGridCombat(null);
        }

        public UnitGridCombat GetUnitGridCombat()
        {
            return unitGridCombat;
        }

    }

}

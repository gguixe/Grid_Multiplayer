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

    private void Start()
    {
        Grid<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid(); //We get combat grid
        grid.GetXY(unitGridCombat.GetPosition(), out int unitX, out int unitY); //We get unit position
        GridPathfindingSystem.GridPathfinding gridPathfinding = GameHandler_GridCombatSystem.Instance.gridPathfinding;

        //Set entire tilemap to invisible
        GameHandler_GridCombatSystem.Instance.GetMovementTilemap().SetAllTilemapSprite(MovementTilemap.TilemapObject.TilemapSprite.None);
        
        int maxMoveDistance = 3; //We want to limit the distance each unit can move
        for (int x=unitX - maxMoveDistance; x < unitX + maxMoveDistance; x++) 
        {
            for (int y = unitY - maxMoveDistance; y < unitY + maxMoveDistance; y++)
            {

                Debug.Log(gridPathfinding.IsWalkable(x, y));

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
        if (Input.GetMouseButtonDown(0))
        {
            unitGridCombat.MoveTo(UtilsClass.GetMouseWorldPosition(), () => {}); //With 3D game we should change this function
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

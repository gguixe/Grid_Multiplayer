//DESC: Grid combat system (basic)
//NAME: Gerard Guix�
//DATE: 06/2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridCombatSystem : MonoBehaviour
{
    [SerializeField] private UnitGridCombat[] unitGridCombatArray;

    private State state;
    private UnitGridCombat unitGridCombat;
    private List<UnitGridCombat> blueTeamList;
    private List<UnitGridCombat> redTeamList;
    private int blueTeamActiveUnitIndex;
    private int redTeamActiveUnitIndex;
    private bool canMoveThisTurn; //We should modifiy this AC system later
    private bool canAttackThisTurn;

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
        blueTeamList = new List<UnitGridCombat>();
        redTeamList = new List<UnitGridCombat>();
        blueTeamActiveUnitIndex = -1;
        redTeamActiveUnitIndex = -1;

        //Set all Units on their grid position
        foreach (UnitGridCombat unitGridCombat in unitGridCombatArray)
        {
            GameHandler_GridCombatSystem.Instance.GetGrid().GetGridObject(unitGridCombat.GetPosition()).SetUnitGridCombat(unitGridCombat); //This only uses the "center" of the unit (not all the space they're occupying)

            if(unitGridCombat.GetTeam() == UnitGridCombat.Team.Blue)
            {
                blueTeamList.Add(unitGridCombat);
            } 
            else
            {
                redTeamList.Add(unitGridCombat);
            }
        }
        SelectNextActiveUnit();
        UpdateValidMovePosition();
    }

    //First time will pick blue team, then will go blue until all units moved, and then red until all units moved.
    private void SelectNextActiveUnit()
    {
        //We want to repeat the walkable raycast after every unit has finished its movement (dynamic obstacles)
        GameHandler_GridCombatSystem.Instance.GetGridPathfinding().RaycastWalkable();

        if (unitGridCombat == null || unitGridCombat.GetTeam() == UnitGridCombat.Team.Red) 
        {
            unitGridCombat = GetNextActiveUnit(UnitGridCombat.Team.Blue);
        }
        else
        {
            unitGridCombat = GetNextActiveUnit(UnitGridCombat.Team.Red);
        }

        //Workaround for bug (we want to set the space of the unit walkable before selecting unit)
        //Vector3 unitPos = unitGridCombat.GetPosition();
        //GameHandler_GridCombatSystem.Instance.GetGridPathfinding().SetWalkable((int)unitPos.x, (int)unitPos.y, true);

        GameHandler_GridCombatSystem.Instance.SetCameraFollowPosition(unitGridCombat.GetPosition());
        canMoveThisTurn = true;
        canAttackThisTurn = true;
    }

    private UnitGridCombat GetNextActiveUnit(UnitGridCombat.Team team)
    {
        if (team == UnitGridCombat.Team.Blue)
        {
            blueTeamActiveUnitIndex = (blueTeamActiveUnitIndex + 1) % blueTeamList.Count;
            return blueTeamList[blueTeamActiveUnitIndex];
        } else
        {
            redTeamActiveUnitIndex = (redTeamActiveUnitIndex + 1) % redTeamList.Count;
            return redTeamList[redTeamActiveUnitIndex];
        }
    }

    private void UpdateValidMovePosition()
    {
        Debug.Log("//////////////////////////////////////////////START OF UPDATE");
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

        int maxMoveDistance = unitGridCombat.GetmaxMoveDistance(); //We want to limit the distance each unit can move
        for (int x=unitX - maxMoveDistance; x < unitX + maxMoveDistance; x++) 
        {
            for (int y = unitY - maxMoveDistance; y < unitY + maxMoveDistance; y++)
            {
                //Debug.Log(gridPathfinding.IsWalkable(x, y));
                //THIS IS ONLY WORKING WHEN WE ARE NOT ON THE EDGE OF THE GRID (BUG), IF THE UNIT HAS A BOX COLLIDER IT ALSO DOESN'T WORK
                //GridPathfinding.GetMapWidth,GridPathfinding.GetMapHeight
                
                Debug.Log("VALOR X " + x);
                Debug.Log("VALOR Y " + y);

                if(x >= 0 && x < gridPathfinding.GetMapWidth() && y >= 0 && y < gridPathfinding.GetMapHeight()) //Ignore when outside grid limits
                { 
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
        Debug.Log("MAXIMO MAPA HEIGHT " + gridPathfinding.GetMapHeight());
        Debug.Log("MAXIMO MAPA WIDTH " + gridPathfinding.GetMapWidth());
        Debug.Log("//////////////////////////////////////////////END OF UPDATE");
    }

    private void Update()
    {
        switch (state)
        {
            case State.Normal:
                if (Input.GetMouseButtonDown(0))
                {
                    Grid<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
                    GridObject gridObject = grid.GetGridObject(UtilsClass.GetMouseWorldPosition());
                    //Check if clicking on unit position
                    if(gridObject.GetUnitGridCombat() != null)
                    {
                        //Clicked on unit
                        //Check if target is enemy (different team)
                        if (unitGridCombat.IsEnemy(gridObject.GetUnitGridCombat()))
                        {
                            if (unitGridCombat.CanAttackUnit(gridObject.GetUnitGridCombat())) //Can attack
                            {
                                if (canAttackThisTurn) { //Check if AC still reamaining
                                    canAttackThisTurn = false;
                                    //Clicked on Enemy of current unit (we attack)
                                    unitGridCombat.AttackUnit(gridObject.GetUnitGridCombat(), () => { state = State.Normal; });
                                    CodeMonkey.CMDebug.TextPopupMouse("PIUM PIUM PIUM!");
                                    TestTurnOver();
                                }
                            }
                            else
                            {
                                //Cannot attack enemy (distance)
                                //Debug.Log("ENEMY " + unitGridCombat + " too FAR AWAY");
                                CodeMonkey.CMDebug.TextPopupMouse("Cannot attack!");
                            }
                            break; //If we try to attack we break (so we dont move)
                        }
                        else
                        {
                            //Not enemy
                        }
                    }
                    else
                    {
                        //Not unit here
                    }

                    //If the grid position is set as valid we allow movement (we get the grid cell on mouse position and check if cell is valid)
                    if (gridObject.GetIsValidMovePosition())
                    {
                        if (canMoveThisTurn) //We check if movement already happened
                        {
                            canMoveThisTurn = false;
                            //Valid Move Position
                            state = State.Waiting;
                            grid.GetGridObject(unitGridCombat.GetPosition()).ClearUnitGridCombat(); //Remove unit from origin/current cell
                            gridObject.SetUnitGridCombat(unitGridCombat); //Tell target cell that this unit is set there
                            unitGridCombat.MoveTo(UtilsClass.GetMouseWorldPosition(), () => { 
                                state = State.Normal;
                                TestTurnOver(); //check if end of turn
                            }); //With 3D game we should change this function //We use delegate on function to update state and positions
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ForceTurnOver();
                }

                break;
            case State.Waiting:
                break;
        }
    }

    private void TestTurnOver()
    {
        if(!canMoveThisTurn && !canAttackThisTurn)
        {
            ForceTurnOver();
        }
    }

    private void ForceTurnOver()
    {
        SelectNextActiveUnit(); //Go to next unit after movement
        UpdateValidMovePosition();
    }


    public class GridObject //Every grid cell is a grid object
    {

        private Grid<GridObject> grid;
        private int x;
        private int y;
        private bool isValidMovePosition;
        private UnitGridCombat unitGridCombat; //Define which unit is in each cell

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Cinemachine;

public class GameHandler : MonoBehaviour
{

    public static GameHandler Instance { get; private set; }

    [SerializeField] private Transform cinemachineFollowTransform;
    [SerializeField] private TilemapVisual tilemapVisual;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private Grid<EmptyGridObject> grid;
    private Tilemap tilemap;
    public Pathfinding pathfinding;

    private void Awake()
    {
        Instance = this;

        int mapWidth = 40;
        int mapHeight = 25;
        float cellSize = 10f;
        Vector3 origin = new Vector3(0, 0);

        grid = new Grid<EmptyGridObject>(mapWidth, mapHeight, cellSize, origin, (Grid<EmptyGridObject> g, int x, int y) => new TilemapObject(g, x, y));

        pathfinding = new Pathfinding(origin + new Vector3(1, 1) * cellSize * .5f, new Vector3(mapWidth, mapHeight));
        //pathfinding.RaycastWalkable();

        tilemap = new Tilemap(mapWidth, mapHeight, cellSize, origin);
    }
}

//EMPTY GAME OBJECT
public class EmptyGridObject
{
    private Grid<EmptyGridObject> grid;
    private int x;
    private int y;

    public EmptyGridObject(Grid<EmptyGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }
}

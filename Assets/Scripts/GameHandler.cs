using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Cinemachine;

public class GameHandler : MonoBehaviour
{

    public static GameHandler Instance { get; private set; }

    [SerializeField] private Transform cinemachineFollowTransform;
    //[SerializedField] private MovementTilemapVisual movementTilemapVisual;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    //private Grid<EmptyGridObject> grid;
    //private MovementTilemap movementTilemap;
    public Pathfinding pathfinding;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

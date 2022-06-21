using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class DemoTilemap : MonoBehaviour
{
    [SerializeField] private TilemapVisual tilemapVisual;
    private Tilemap tilemap;

    private void Start()
    {
        tilemap = new Tilemap(20, 20, 10f, Vector3.zero);
        tilemap.SetTilemapVisual(tilemapVisual);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            tilemap.SetTilemapSprite(mouseWorldPosition, Tilemap.TilemapObject.TilemapSprite.Ground);
        }
    }
}

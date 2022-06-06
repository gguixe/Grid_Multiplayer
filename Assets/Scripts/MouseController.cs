using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq; //Try to learn about this
using UnityEngine;

public class MouseController : MonoBehaviour
{

    public float speed;

    public GameObject characterPrefab;
    private CharacterInfo character;

    private Pathfinder pathFinder;
    private RangeFinder rangeFinder;
    private List<OverlayTile> path = new List<OverlayTile>();
    private List<OverlayTile> inRangeTiles = new List<OverlayTile>();

    private void Start()
    {
        pathFinder = new Pathfinder();
        rangeFinder = new RangeFinder();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        RaycastHit2D? hit = GetFocusedOnTile();

        if (hit.HasValue) //If there's something in GetFocusedOnTile
        {
            OverlayTile overlayTile = hit.Value.collider.gameObject.GetComponent<OverlayTile>(); //Get tile from mouse position
            transform.position = overlayTile.transform.position; //Set tile to position
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = overlayTile.GetComponent<SpriteRenderer>().sortingOrder; //Set correct sorting order
            
            if (Input.GetMouseButtonDown(0))
            {
                overlayTile.GetComponent<OverlayTile>().ShowTile();

                if(character == null)
                {
                    character = Instantiate(characterPrefab).GetComponent<CharacterInfo>();
                    PositionCharacterOnTile(overlayTile); //Position character on tile clicked
                    GetInRangeTiles();
                }
                else
                {
                    path = pathFinder.FindPath(character.activeTile, overlayTile); //Find path with second click
                    overlayTile.GetComponent<OverlayTile>().HideTile();
                }
            }

            if(path.Count > 0)
            {
                MoveAlongPath();
            }
        }
    }

    private void GetInRangeTiles()
    {
        foreach (var item in inRangeTiles)
        {
            item.ShowTile();
        }

        inRangeTiles = rangeFinder.GetTilesInRange(character.activeTile, 3); //MOVEMENT OF CHARACTER

        foreach (var item in inRangeTiles)
        {
            item.ShowTile();
        }
    }

    private void MoveAlongPath()
    {
        var step = speed * Time.deltaTime;
        var zIndex = path[0].transform.position.z;

        character.transform.position = Vector2.MoveTowards(character.transform.position, path[0].transform.position, step);//Need Vector2 to not slow down movement (z should be instantaneous)
        character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, zIndex+1);

        if(Vector2.Distance(character.transform.position, path[0].transform.position) < 0.0001f)
        {
            PositionCharacterOnTile(path[0]);
            path.RemoveAt(0);
        }

        if(path.Count == 0)
        {
            GetInRangeTiles();
        }
    }

    public RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Mouse Position
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First(); //If there's more than one GameObject on top of each other, sort the objects by Z position, take first one.
        }

        return null;
    }

    private void PositionCharacterOnTile(OverlayTile tile) 
    {
        character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, tile.transform.position.z); //Position and sorting order of target tile
        character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        character.activeTile = tile;
    }

}

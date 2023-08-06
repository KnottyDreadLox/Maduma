using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{

    private Grid grid;

    public Vector3Int startCoordinates;

    Vector3Int top = new Vector3Int(-1, -1, 0);
    Vector3Int mid = new Vector3Int(-1, 0, 0);
    Vector3Int bot = new Vector3Int(-1, 1, 0);
    Vector3Int t, m, b;

    Vector3Int ClockwiseTop = new Vector3Int(0, -1, 0);
    Vector3Int ClockwiseMid = new Vector3Int(-1, -1, 0);
    Vector3Int ClockwiseBot = new Vector3Int(-2, -1, 0);

    Vector3Int AntiClockwiseTop = new Vector3Int(0, 1, 0);
    Vector3Int AntiClockwiseMid = new Vector3Int(-1, 1, 0);
    Vector3Int AntiClockwiseBot = new Vector3Int(2, 1, 0);

    Vector3Int activeTileCoordinate;

    [SerializeField]
    private Tilemap baseTilemap;

    [SerializeField]
    private Tilemap overlayTilemap;

    [SerializeField]
    private TileBase overlayTile;

    BoundsInt bounds;
    TileBase[] tilebaseArray;

    Dictionary<Vector3Int, string> removedTiles;


    bool clockwise = false;
    bool anticlockwise = false;


    // Start is called before the first frame update
    void Start()
    {
        grid = gameObject.GetComponent<Grid>();

        t = startCoordinates - top;
        m = startCoordinates - mid;
        b = startCoordinates - bot;

        //Bounds is how big the tiles being used is
        bounds = new BoundsInt(baseTilemap.origin, baseTilemap.size);
        tilebaseArray = baseTilemap.GetTilesBlock(bounds);
        Debug.Log("Tilebase Array is " + tilebaseArray.Length + " tiles.");

        removedTiles = new Dictionary<Vector3Int, string>();    

    }


    public void SwitchState(string _case)
    {
        
        switch (_case)
        {
            case "normal":
                t = activeTileCoordinate - top;
                m = activeTileCoordinate - mid;
                b = activeTileCoordinate - bot;
                Debug.Log("Switched to Normal");
                break;

            case "clockwise":
                t = activeTileCoordinate - ClockwiseTop;
                m = activeTileCoordinate - ClockwiseMid;
                b = activeTileCoordinate - ClockwiseBot;
                Debug.Log("Switched to Clockwise");
                break;

            case "anticlockwise":
                t = activeTileCoordinate - AntiClockwiseTop;
                m = activeTileCoordinate - AntiClockwiseMid;
                b = activeTileCoordinate - AntiClockwiseBot;
                Debug.Log("Switched to Anticlockwise");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        activeTileCoordinate = grid.WorldToCell(mouseWorldPos);
        Sprite activeTileSprite = baseTilemap.GetSprite(activeTileCoordinate);

        if (Input.GetMouseButtonDown(0))
        {
            try
            {

                Debug.Log("This is at position " + activeTileCoordinate);

                //if the selected tile is within the moveable range of t,m,b and not null
                if (activeTileCoordinate == t && activeTileSprite.texture != null || activeTileCoordinate == m && activeTileSprite.texture != null || activeTileCoordinate == b && activeTileSprite.texture != null)
                {
                    //start counting
                    GameManager.startClock = true;

                    //put the sprite marker on this to know where play is on the second overlay
                    overlayTilemap.SetTile(activeTileCoordinate, overlayTile);

                    //If this tile is Endgame Tile, Finish level
                    if (activeTileSprite.name == "Finish")
                    {
                        GameManager.CompleteLevel();
                    }

                    //if this tile is a ressurection tile
                    if (activeTileSprite.name.Contains("Return"))
                    {
                        ReturnTiles(activeTileSprite.name);
                    }

                    if (activeTileSprite.name.Contains("Clockwise"))
                    {
                        if (clockwise == true)
                        {
                            clockwise = false;
                            anticlockwise = false;
                            GameManager.TurnCameraDefault();
                        }

                        else
                        {
                            clockwise = true;
                            GameManager.TurnCameraClockwise();
                            SwitchState("clockwise");
                        }
                    }

                    if (activeTileSprite.name.Contains("Anticlockwise"))
                    {
                        if(anticlockwise == true)
                        {
                            clockwise = false;
                            anticlockwise = false;
                            GameManager.TurnCameraDefault();
                        }

                        else 
                        {
                            anticlockwise = true;
                            GameManager.TurnCameraAntiClockwise();
                            SwitchState("anticlockwise");
                        }
                    }

                    //Where to move next conditions
                    else
                    {
                        if(clockwise == true)
                        {
                            SwitchState("clockwise");
                        }

                        if(anticlockwise == true)
                        {
                            SwitchState("anticlockwise");
                        }

                        if(anticlockwise == false && anticlockwise == false)
                        {
                            SwitchState("normal");
                        }
                    }

                    foreach (var position in baseTilemap.cellBounds.allPositionsWithin)
                    {
                        //if nothing exists, do nothing
                        if (!baseTilemap.HasTile(position))
                        {
                            continue;
                        }

                        //if this tile is the same as the active tile, ignore it
                        else if (position == activeTileCoordinate)
                        {
                            Sprite otherTiles = baseTilemap.GetSprite(position);
                            Debug.Log("Active tile is at " + position + " and is " + otherTiles.name);
                        }

                        //else remove all tiles that match the active tile
                        //tiles are added to dictionary to be revived
                        else
                        {
                            Sprite otherTile = baseTilemap.GetSprite(position);

                            if (otherTile.name == activeTileSprite.name)
                            {
                                removedTiles.Add(position, otherTile.name);
                                baseTilemap.SetTile(position, null);
                            }

                        }
                    }

                    //Debug.Log("This is " + activeTileCoordinate + " Top is " + t + " Mid is " + m + " Bot is " + b);

                }

                removedTiles.ToList().ForEach(x =>
                {
                    Debug.Log("Removed tiles " + x.Value + " at position " + x.Key);
                });
            }
            catch (Exception e)
            {

                Debug.LogError(e);
            }
        }
    }

    public void ReturnTiles(string tileToReturn)
    {
        removedTiles.ToList().ForEach(x =>
        {
            Debug.Log("Removed tiles " + x.Value + " at position " + x.Key);

            if(x.Value == tileToReturn.Replace("Return", ""))
            {
                switch (tileToReturn)
                {
                    case "RedReturn":
                        baseTilemap.SetTile(x.Key, Resources.Load<TileBase>("Tiles/Red"));
                        break;
                    case "BlueReturn":
                        baseTilemap.SetTile(x.Key, Resources.Load<TileBase>("Tiles/Blue"));
                        break;
                    case "YellowReturn":
                        baseTilemap.SetTile(x.Key, Resources.Load<TileBase>("Tiles/Yellow"));
                        break;
                    case "GreenReturn":
                        baseTilemap.SetTile(x.Key, Resources.Load<TileBase>("Tiles/Green"));
                        break;
                    case "CyanReturn":
                        baseTilemap.SetTile(x.Key, Resources.Load<TileBase>("Tiles/Cyan"));
                        break;
                    case "PinkReturn":
                        baseTilemap.SetTile(x.Key, Resources.Load<TileBase>("Tiles/Pink"));
                        break;
                    case "DarkGreenReturn":
                        baseTilemap.SetTile(x.Key, Resources.Load<TileBase>("Tiles/DarkGreen"));
                        break;
                    case "OrangeReturn":
                        baseTilemap.SetTile(x.Key, Resources.Load<TileBase>("Tiles/Orange"));
                        break;
                    default:
                        baseTilemap.SetTile(x.Key, null);
                        break;
                }
            }
        });
    }
}



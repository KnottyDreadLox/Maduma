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

    //--------------- Variables --------------------

    [SerializeField]
    private GameManager gameManager;

    private Grid grid;

    public Vector3Int startCoordinates;

    //Temporary Active States
    Vector3Int t, m, b;

    //default states
    Vector3Int top = new Vector3Int(-1, -1, 0);
    Vector3Int mid = new Vector3Int(-1, 0, 0);
    Vector3Int bot = new Vector3Int(-1, 1, 0);

    //Offset when Clockwise
    Vector3Int ClockwiseTop = new Vector3Int(-1, 1, 0);
    Vector3Int ClockwiseMid = new Vector3Int(0, 1, 0);
    Vector3Int ClockwiseBot = new Vector3Int(1, 1, 0);

    //Offset when AntiClockwise
    Vector3Int AntiClockwiseTop = new Vector3Int(1, -1, 0);
    Vector3Int AntiClockwiseMid = new Vector3Int(0, -1, 0);
    Vector3Int AntiClockwiseBot = new Vector3Int(-1, -1, 0);

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


    //bool clockwise = false;
    //bool anticlockwise = false;

    private string state;

    //--------------- Encapsulation --------------------

    public Tilemap BaseTilemap { get => baseTilemap; set => baseTilemap = value; }
    public Tilemap OverlayTilemap { get => overlayTilemap; set => overlayTilemap = value; }
    public TileBase OverlayTile { get => overlayTile; set => overlayTile = value; }


    // Start is called before the first frame update
    void Start()
    {
        grid = gameObject.GetComponent<Grid>();

        t = startCoordinates - top;
        m = startCoordinates - mid;
        b = startCoordinates - bot;

        state = "normal";

        //Bounds is how big the tiles being used is
        bounds = new BoundsInt(BaseTilemap.origin, BaseTilemap.size);
        tilebaseArray = BaseTilemap.GetTilesBlock(bounds);
        Debug.Log("Tilebase Array is " + tilebaseArray.Length + " tiles.");

        removedTiles = new Dictionary<Vector3Int, string>();    

    }


    public void SwitchState(string _case)
    {
        
        switch (_case)
        {
            case "normal":
                state = "normal";
                t = activeTileCoordinate - top;
                m = activeTileCoordinate - mid;
                b = activeTileCoordinate - bot;
                Debug.Log("<color=lime> Switched to Normal </color>");
                break;

            case "clockwise":
                state = "clockwise";
                t = activeTileCoordinate - ClockwiseTop;
                m = activeTileCoordinate - ClockwiseMid;
                b = activeTileCoordinate - ClockwiseBot;
                Debug.Log("<color=lime> Switched to Clockwise </color>");
                break;

            case "anticlockwise":
                state = "anticlockwise";
                t = activeTileCoordinate - AntiClockwiseTop;
                m = activeTileCoordinate - AntiClockwiseMid;
                b = activeTileCoordinate - AntiClockwiseBot;
                Debug.Log("<color=lime> Switched to Anticlockwise </color>");
                break;

            case "inverse":
                state = "inverse";
                t = activeTileCoordinate + top;
                m = activeTileCoordinate + mid;
                b = activeTileCoordinate + bot;
                Debug.Log("<color=lime> Switched to Inverse </color>");
                break;
        }

        Debug.Log("<color=red> Next Coordinates at: </color>" + "<color=yellow> Top: </color>" + t + " | <color=lime>Mid: </color>" + m + " | <color=cyan>Bottom: </color>" + b);

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        activeTileCoordinate = grid.WorldToCell(mouseWorldPos);
        Sprite activeTileSprite = BaseTilemap.GetSprite(activeTileCoordinate);

        if (Input.GetMouseButtonDown(0))
        {
            try
            {

                Debug.Log("This is at position " + activeTileCoordinate);

                //if the selected tile is within the moveable range of t,m,b and not null
                if (activeTileCoordinate == t && activeTileSprite.texture != null || activeTileCoordinate == m && activeTileSprite.texture != null || activeTileCoordinate == b && activeTileSprite.texture != null)
                {
                    //start counting
                    gameManager.StartClock = true;

                    //put the sprite marker on this to know where play is on the second overlay
                    OverlayTilemap.SetTile(activeTileCoordinate, OverlayTile);



                    //Check if tile is a special tile
                    if (!activeTileSprite.name.Contains("Anticlockwise") && !activeTileSprite.name.Contains("Clockwise"))
                    {
                        //Destroy other tiles of same colour
                        foreach (var position in BaseTilemap.cellBounds.allPositionsWithin)
                        {
                            //if nothing exists, do nothing
                            if (!BaseTilemap.HasTile(position))
                            {
                                continue;
                            }

                            //if this tile is the same as the active tile, ignore it
                            else if (position == activeTileCoordinate)
                            {
                                Sprite otherTiles = BaseTilemap.GetSprite(position);
                                Debug.Log("<color=green> Active tile is at </color> " + position + " and is " + otherTiles.name);
                            }

                            //else remove all tiles that match the active tile
                            //tiles are added to dictionary to be revived
                            else
                            {
                                Sprite otherTile = BaseTilemap.GetSprite(position);

                                if (otherTile.name == activeTileSprite.name)
                                {
                                    removedTiles.Add(position, otherTile.name);
                                    BaseTilemap.SetTile(position, null);
                                }

                            }
                        }
                    }

                    //If this tile is Endgame Tile, Finish level
                    if (activeTileSprite.name == "Finish")
                    {
                        gameManager.CompleteLevel();
                    }

                    //if this tile is a ressurection tile
                    if (activeTileSprite.name.Contains("Return"))
                    {
                        ReturnTiles(activeTileSprite.name);
                    }

                    //if this is the Clockwise Tile
                    if (activeTileSprite.name.Contains("Clockwise"))
                    {

                        gameManager.TurnCameraClockwise();
                        SwitchState("clockwise");

                    }

                    //if this is the Anticlockwise Tile
                    if (activeTileSprite.name.Contains("Anticlockwise"))
                    {

                        gameManager.TurnCameraAntiClockwise();
                        SwitchState("anticlockwise");

                    }

                    //Where to move next conditions
                    else
                    {
                        if(state == "clockwise")
                        {
                            SwitchState(state);
                        }

                        if(state == "anticlockwise")
                        {
                            SwitchState(state);
                        }

                        if(state == "normal")
                        {
                            SwitchState(state);
                        }
                    }

                    //Debug.Log("This is " + activeTileCoordinate + " Top is " + t + " Mid is " + m + " Bot is " + b);

                }

                //removed tiles Debug
                //removedTiles.ToList().ForEach(x =>
                //{
                //    Debug.Log("Removed tiles " + x.Value + " at position " + x.Key);
                //});
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
                        BaseTilemap.SetTile(x.Key, Resources.Load<TileBase>("Tiles/Red"));
                        break;
                    case "BlueReturn":
                        BaseTilemap.SetTile(x.Key, Resources.Load<TileBase>("Tiles/Blue"));
                        break;
                    case "YellowReturn":
                        BaseTilemap.SetTile(x.Key, Resources.Load<TileBase>("Tiles/Yellow"));
                        break;
                    case "GreenReturn":
                        BaseTilemap.SetTile(x.Key, Resources.Load<TileBase>("Tiles/Green"));
                        break;
                    case "CyanReturn":
                        BaseTilemap.SetTile(x.Key, Resources.Load<TileBase>("Tiles/Cyan"));
                        break;
                    case "PinkReturn":
                        BaseTilemap.SetTile(x.Key, Resources.Load<TileBase>("Tiles/Pink"));
                        break;
                    case "DarkGreenReturn":
                        BaseTilemap.SetTile(x.Key, Resources.Load<TileBase>("Tiles/DarkGreen"));
                        break;
                    case "OrangeReturn":
                        BaseTilemap.SetTile(x.Key, Resources.Load<TileBase>("Tiles/Orange"));
                        break;
                    default:
                        BaseTilemap.SetTile(x.Key, null);
                        break;
                }
            }
        });
    }


    private void MakeMatrix(int[]top, int[]mid, int[]bot)
    {

    }
}



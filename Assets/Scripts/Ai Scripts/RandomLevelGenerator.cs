using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomLevelGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase[] tiles;

    public TileBase[] startTiles;
    public TileBase[] endTiles;

    //Values given based on Tiles array lenght. 
    private int width;
    private int height;

    private Matrix2D tileMatrix;

    //Must be Awake to trigger coordinates for GameManager
    private void Awake()
    {
        height = tiles.Length;
        width = tiles.Length;

        tileMatrix = new Matrix2D(width, height); // Initialize the matrix

        PlaceStartAndEndTiles();
    }

    void Start()
    {
        PlaceRandomTiles();
    }

    void PlaceRandomTiles()
    {
        /*
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                tilemap.SetTile(tilePosition, GetRandomTile());
                tileMatrix[x, y] = tilePosition; // Store position in the matrix
                string tileName = tilemap.GetTile(tilePosition).name;
                tileMatrix.SetName(x, y, tileName);
            }
        }

        Debug.Log("Tile Matrix:");

        for (int row = 0; row < height; row++)
        {
            string rowString = "";
            for (int col = 0; col < width; col++)
            {
                rowString += tileMatrix.GetName(col, row) + " ";
            }

            //Debug.Log(rowString);
        }
        */
    }

    void PlaceStartAndEndTiles()
    {
        // Place start tile on the left
        Vector3Int startPos = new Vector3Int(-1, Random.Range(0, height), 0);
        tilemap.SetTile(startPos, GetRandomTile(startTiles));

        Debug.Log("Start Position: " + startPos);

        GameManager.startCoordinates = startPos; // Set start position in GameManager
        GameManager.endCoordinates = new List<Vector3Int>(); // Clear the list before populating it

        // Place end tiles
        foreach (TileBase endTile in endTiles)
        {
            Vector3Int endPos;
            do
            {
                endPos = new Vector3Int(width, Random.Range(0, height), 0);
            }
            while (GameManager.endCoordinates.Contains(endPos)); // Check if the position already exists

            tilemap.SetTile(endPos, endTile);
            Debug.Log("End Position: " + endPos);
            GameManager.endCoordinates.Add(endPos); // Add unique end position to GameManager
        }
    }

    TileBase GetRandomTile(TileBase[] tileArray)
    {
        return tileArray[Random.Range(0, tileArray.Length)];
    }

    TileBase GetRandomTile()
    {
        return tiles[Random.Range(0, tiles.Length)];
    }
}
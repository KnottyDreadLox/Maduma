using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GAMapGenerator : MonoBehaviour
{

    private int fixedStartPos = 3;
    
    [SerializeField]
    private int initialMatricesToGenerate;

    //Game tilemap
    public Tilemap tilemap;

    MatrixGA matrixGA;

    [Header("Available Tileset to Generate.")]
    [Tooltip("This array contains all the tiles that the level generator will use. Include any tiles, even special tiles. If you add special tiles, add them to the below specials")]
    //Dna genetics 
    public DNA[] dnaArray;

    [Header("Start Tiles")]
    [Tooltip("More start tiles equals more starting locations.")]
    //Start and end tiles
    public TileBase[] startTiles;


    [Header("End Tiles")]
    [Tooltip("More end tiles equals more end locations.")]
    public TileBase[] endTiles;


    [Header("Special DNA Tiles")]
    [Tooltip("Put the special Empty tile here so the engine can recognize which one to ignore when performing fitness check.")]
    public DNA specialEmptyDna;

    //Values given based on Tiles array lenght. 
    private int width;
    private int height;

    private DNAMatrix geneticMatrix;

    //This will be the memory of the generated matrices
    private List<DNAMatrix> DNAMatrices = new List<DNAMatrix>();

    //counts button clicks - this will be used for keeping track of mutations
    int mutationBtnCounter = 0;

    //Must be Awake to trigger coordinates for GameManager
    private void Awake()
    {
        //set height + width to be lenght of the array of tiles

        //first check if the List has the special empty tile, if it does, count 1 less, else set them to be equal.
        if (!dnaArray.ToList().Contains(specialEmptyDna))
        {
            height = dnaArray.Length - 1;
            width = dnaArray.Length - 1;
        }
        else
        {
            height = dnaArray.Length;
            width = dnaArray.Length;
        }

    }


    void Start()
    {
        //Creates initial grid which is random
        matrixGA = gameObject.GetComponent<MatrixGA>();

    }

    public static string ApplyColourToChar(char character)
    {
        switch (character)
        {
            case 'R':
                return "<color=red>" + character + "</color>";
            case 'G':
                return "<color=green>" + character + "</color>";
            case 'B':
                return "<color=blue>" + character + "</color>";
            case 'Y':
                return "<color=yellow>" + character + "</color>";
            case 'O':
                return "<color=orange>" + character + "</color>";
            default:
                Debug.LogWarning("Unsupported color: " + character);
                return character.ToString();
        }
    }

    void PlaceStartAndEndTileFixed()
    {


        Vector3Int startPos = new Vector3Int(-1, height - fixedStartPos, 0);

        // Place start tile on the left
        tilemap.SetTile(startPos, GetRandomStartTile(startTiles));

        Vector3Int endPos = new Vector3Int(width, height - fixedStartPos, 0);

        tilemap.SetTile(endPos, GetRandomEndTile(endTiles));


        //Debug.Log("Start Position: " + startPos);

        GameManager.startCoordinates = startPos; // Set start position in GameManager
        GameManager.endCoordinatesStatic = endPos; // Set start position in GameManager


    }

    void PlaceStartAndEndTilesRandomly()
    {
        // Place start tile on the left
        Vector3Int startPos = new Vector3Int(-1, Random.Range(0, height), 0);
        tilemap.SetTile(startPos, GetRandomStartTile(startTiles));

        //Debug.Log("Start Position: " + startPos);

        GameManager.startCoordinates = startPos; // Set start position in GameManager
        GameManager.endCoordinatesDynamic = new List<Vector3Int>(); // Clear the list before populating it

        // Place end tiles
        foreach (TileBase endTile in endTiles)
        {
            Vector3Int endPos;
            do
            {
                endPos = new Vector3Int(width, Random.Range(0, height), 0);
            }
            while (GameManager.endCoordinatesDynamic.Contains(endPos)); // Check if the position already exists

            tilemap.SetTile(endPos, endTile);

            //Debug.Log("End Position: " + endPos);
            GameManager.endCoordinatesDynamic.Add(endPos); // Add unique end position to GameManager
        }
    }

    //Get a random starting position for the Start Tile
    TileBase GetRandomStartTile(TileBase[] tileArray)
    {
        return tileArray[Random.Range(0, tileArray.Length)];
    }

    TileBase GetRandomEndTile(TileBase[] tileArray)
    {
        return tileArray[Random.Range(0, tileArray.Length)];
    }


    //Get a random DNA from the DNA matrix
    public DNA GetRandomDNA()
    {
        return dnaArray[Random.Range(0, dnaArray.Length)];
    }

    private DNAMatrix GenerateRandomLevel()
    {
        Debug.Log("<color=green> GENNERATING RANDOM LEVEL </color>");

        tilemap.ClearAllTiles();

        //Creates a genetic matrix of this level.
        geneticMatrix = new DNAMatrix(width, height);                   // Initialize the matrix
        DNAMatrices.Add(geneticMatrix);                                 //saves this matrix layout in memory.

        PlaceStartAndEndTileFixed();
        PlaceRandomTiles();

        return geneticMatrix;
    }


    void PlaceRandomTiles()
    {
        //Place randomly selected tiles (colours) around the board
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);          //gets this position and set a tile down
                DNA dna = GetRandomDNA();                                   //picks a random DNA out of the DNA pool (the array of AiTiles)

                tilemap.SetTile(tilePosition, dna.tile);                    //Sets down a tile at this position that is associated to this DNA.

                geneticMatrix[x, y] = tilePosition;                         //Store position in the geneticMatrix
                geneticMatrix.SetDNA(x, y, dna);                            //sets the DNA at the genetic geneticMatrix's position
            }
        }


        //This code is for debug of the postions of tiles kinda long...
        Debug.Log("Tile Matrix:");


        for (int row = 0; row < height; row++)
        {
            string rowString = "";
            for (int col = 0; col < width; col++)
            {
                string dnaName = geneticMatrix.GetDNA(col, row).DNAName;
                char firstLetter = dnaName[0];

                string colourfulText = ApplyColourToChar(firstLetter);

                rowString += colourfulText + " ";
            }

            Debug.Log(rowString);
        }
    }


    private void GeneticallyGenerateLevel()
    {
        Debug.Log("<color=green> GENNERATING RANDOM LEVEL </color>");

        tilemap.ClearAllTiles();

        PlaceStartAndEndTileFixed();

        //Add the new genetically generated matrix to the List
        Debug.Log(DNAMatrices.Count);
        Debug.Log(DNAMatrices.Count - 1);

        DNAMatrix childMatrix = matrixGA.GeneticallyMutateMatrix(DNAMatrices[DNAMatrices.Count - 1], DNAMatrices[DNAMatrices.Count - 2], dnaArray);
        DNAMatrices.Add(childMatrix);

        PlaceTilesOnTilemap(childMatrix);

    }

    private void PlaceTilesOnTilemap(DNAMatrix dnaMatrix)
    {
        for (int x = 0; x < dnaMatrix.Rows; x++)
        {
            for ( int y = 0; y < dnaMatrix.Columns; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);          //gets this position and set a tile down
                DNA dna = dnaMatrix.GetDNA(x, y);                         //picks a random DNA out of the DNA pool (the array of AiTiles)

                tilemap.SetTile(tilePosition, dna.tile);                    //Sets down a tile at this position that is associated to this DNA.
            }
        }
    }

    public void Mutate()
    {
        //The first one is random auto generated, the second is forcefully also randomly generated
        //the rest that follow are genetic mutations of the first 2 randoms
        if (DNAMatrices.Count <= 2)
        {
            GenerateRandomLevel();
        }
        else
        {
            GeneticallyGenerateLevel();
        }
    }


    public void LoadGame()
    {

        List<DNAMatrix> randomMatrices = new List<DNAMatrix>();

        randomMatrices = GenerateRandomMatrices(initialMatricesToGenerate);

        Debug.Log("<color=green> Generated " + randomMatrices.Count + " random DNAMatrices </color>");

        List<DNAMatrix> validMatrices = new List<DNAMatrix>();

        validMatrices = PerformFitnessCheck(randomMatrices);

        Debug.Log("<color=green> There are " + validMatrices.Count + " valid playable DNAMatrices </color>");


        PlaceStartAndEndTileFixed();
        PlaceTilesOnTilemap(validMatrices[Random.Range(0, validMatrices.Count)]);
    }


    private List<DNAMatrix> PerformFitnessCheck(List<DNAMatrix> randomMatrices)
    {

        List<DNAMatrix> validMatrices = new List<DNAMatrix>();

        foreach(DNAMatrix matrix in randomMatrices)
        {
            if (isPlayable(matrix))
            {
                validMatrices.Add(matrix);
            }
        }

        return validMatrices;
    }

    
    private bool isPlayable(DNAMatrix matrix)
    {

        List<int> tileColoursActived = new List<int>();

        //y pos follows the player, essentially acts as the 'active position'
        int yPos = GameManager.startCoordinates.y;

        //TODO - Continue fitness model to check neighbouring tiles
        for(int x = 0; x < matrix.Columns; x++)
        {
            
            //check if next tiles are valid. Also ensure that the next tile is not the empty tile '0'

            if (!tileColoursActived.Contains(matrix.GetDNA(x, yPos).id) && matrix.GetDNA(x, yPos).id != 0)
            {
                tileColoursActived.Add(matrix.GetDNA(x, yPos).id);
            }

            else if (!tileColoursActived.Contains(matrix.GetDNA(x, yPos + 1).id) && matrix.GetDNA(x, yPos).id != 0)
            {
                tileColoursActived.Add(matrix.GetDNA(x, yPos + 1).id);
                yPos++;
            }

            else if (!tileColoursActived.Contains(matrix.GetDNA(x, yPos - 1).id) && matrix.GetDNA(x, yPos).id != 0)
            {
                tileColoursActived.Add(matrix.GetDNA(x, yPos - 1).id);
                yPos--;
            }

        }

        if(tileColoursActived.Count == matrix.Columns)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public List<DNAMatrix> GenerateRandomMatrices(int matricesToGenerate)
    {
        List<DNAMatrix> randomMatrices = new List<DNAMatrix>();

        for(int i =0; i < matricesToGenerate; i++)
        {
            randomMatrices.Add(GenerateRandomLevel());
        }

        return randomMatrices;
    }
    
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;
using Random = UnityEngine.Random;

public class AiLevelGenerator : MonoBehaviour
{

    [SerializeField] private Tilemap gameTilemap;

    [SerializeField] private int width;
    [SerializeField] private int height;

    private AiNode[,] tileGrid;

    List<AiNode> aiNodes = new List<AiNode>();

    List<Vector2Int> toCollapse = new List<Vector2Int>();
    Vector2Int[] offsets = new Vector2Int[]
    {
        new Vector2Int(0, 1),        //top
        new Vector2Int(0, -1),       //bottom
        new Vector2Int(1, 0),        //right
        new Vector2Int(-1, 0)        //left
    };

    // Start is called before the first frame update
    void Start()
    {
        tileGrid = new AiNode[width, height];

        //Debug.Log("Started Generating ! ");

        //CollapseWorld();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Generate Nodes"))
        {

            Debug.Log("Started Generating ! ");
            CollapseWorld();

        }
    }


    private void CollapseWorld()
    {
        //clear list for fresh start
        toCollapse.Clear();

        //origin node 
        toCollapse.Add(new Vector2Int(width / 2, height / 2));

        while(toCollapse.Count > 0)
        {
            int x = toCollapse[0].x;
            int y = toCollapse[0].y;

            //Pass over nodes
            List<AiNode> potentialNodes = new List<AiNode>(aiNodes);
            
            //loop each neighbour
            for(int i = 0; i < offsets.Length; i++)
            {
                //this position + offset = neighbour positions
                Vector2Int neighbour = new Vector2Int(x + offsets[i].x, y + offsets[i].y);

                //check if it fits in the grid
                if (isInGrid(neighbour))
                {
                    AiNode neighbourNode = tileGrid[neighbour.x, neighbour.y];

                    if(neighbour != null)
                    {
                        switch (i)
                        {
                            case 0:
                                RemoveInvalidNodes(potentialNodes, neighbourNode.Bottom.CompatibleNodes);
                                break;

                            case 1:
                                RemoveInvalidNodes(potentialNodes, neighbourNode.Top.CompatibleNodes);
                                break;

                            case 2:
                                RemoveInvalidNodes(potentialNodes, neighbourNode.Left.CompatibleNodes);
                                break;

                            case 3:
                                RemoveInvalidNodes(potentialNodes, neighbourNode.Right.CompatibleNodes);
                                break;
                        }
                    }
                    else
                    {
                        if (!toCollapse.Contains(neighbour))
                        {
                            toCollapse.Add(neighbour);
                        }
                    }
                }
            }

            //After whittling collapse the this cycle

            if(potentialNodes.Count < 1)
            {
                tileGrid[x, y] = aiNodes[0];
                Debug.LogWarning("Attempted collapse on " + x + "," + y + " found no compatible nodes.");
            }
            else
            {
                //Pick random
                tileGrid[x, y] = potentialNodes[Random.Range(0, potentialNodes.Count)];
            }

            gameTilemap.SetTile(new Vector3Int(x, y, 0), tileGrid[x,y].tile);

            toCollapse.RemoveAt(0);


        }
    }

    //ensure this can fit in the grid
    private bool isInGrid(Vector2Int vector2Int)
    {
        if(vector2Int.x > - 1 && vector2Int.x < width && vector2Int.y > -1 && vector2Int.y < height)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //compare 2 node lists and removes any invalid nodes
    private void RemoveInvalidNodes(List<AiNode> potentialNodes, List<AiNode> validNodes)
    {
        for (int i = potentialNodes.Count - 1; i > -1; i--)
        {
            if (!validNodes.Contains(potentialNodes[i]))
            {
                potentialNodes.RemoveAt(i);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 targetPosition;

    // Define a Node class for storing information about each tile on the grid
    class Node
    {
        public Vector3 position;
        public Node parent;
        public int gCost;
        public int hCost;
        public int FCost { get { return gCost + hCost; } }

        public Node(Vector3 _pos)
        {
            position = _pos;
            parent = null;
            gCost = 0;
            hCost = 0;
        }
    }

    // A* algorithm function
    List<Vector3> FindPath(Vector3 start, Vector3 target)
    {
        List<Vector3> path = new List<Vector3>();

        List<Node> openSet = new List<Node>();
        HashSet<Vector3> closedSet = new HashSet<Vector3>();

        Node startNode = new Node(start);
        Node targetNode = new Node(target);

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || (openSet[i].FCost == currentNode.FCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode.position);

            if (currentNode.position == targetNode.position)
            {
                path = RetracePath(startNode, targetNode);
                break;
            }

            foreach (Vector3 neighborPos in GetNeighbors(currentNode.position))
            {
                if (!closedSet.Contains(neighborPos))
                {
                    int newCostToNeighbor = currentNode.gCost + GetDistance(currentNode.position, neighborPos);
                    Node neighborNode = GetNode(neighborPos, openSet);
                    if (neighborNode == null || newCostToNeighbor < neighborNode.gCost)
                    {
                        Vector3 decision = MakeDecision(neighborPos); // Make a decision for the neighbor position
                        neighborNode = GetNode(decision, openSet);
                        if (neighborNode == null)
                        {
                            neighborNode = new Node(decision);
                            openSet.Add(neighborNode);
                        }
                        neighborNode.gCost = newCostToNeighbor;
                        neighborNode.hCost = GetDistance(decision, target);
                        neighborNode.parent = currentNode;
                    }
                }
            }
        }

        return path;
    }

    // Retrace the path from start to end
    List<Vector3> RetracePath(Node startNode, Node endNode)
    {
        List<Vector3> path = new List<Vector3>();
        Node currentNode = endNode;

        while (currentNode != null && currentNode != startNode)
        {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }

        if (currentNode == startNode)
        {
            path.Add(currentNode.position); // Add start node position if it's reached
        }

        path.Reverse();
        return path;
    }

    // Get the distance between two nodes
    int GetDistance(Vector3 nodeA, Vector3 nodeB)
    {
        int dstX = Mathf.Abs(Mathf.RoundToInt(nodeA.x) - Mathf.RoundToInt(nodeB.x));
        int dstY = Mathf.Abs(Mathf.RoundToInt(nodeA.y) - Mathf.RoundToInt(nodeB.y));

        return dstX + dstY;
    }

    // Get neighboring nodes
    List<Vector3> GetNeighbors(Vector3 pos)
    {
        List<Vector3> neighbors = new List<Vector3>();

        // Add neighboring positions here based on your grid logic
        // Example: Add right, above-right, below-right neighbors
        neighbors.Add(new Vector3(pos.x + 1, pos.y, pos.z));
        neighbors.Add(new Vector3(pos.x + 1, pos.y + 1, pos.z));
        neighbors.Add(new Vector3(pos.x + 1, pos.y - 1, pos.z));

        return neighbors;
    }

    // Get a node from a list based on its position
    Node GetNode(Vector3 pos, List<Node> nodeList)
    {
        foreach (Node node in nodeList)
        {
            if (node.position == pos)
            {
                return node;
            }
        }
        return null;
    }

    // Method to make AI decisions
    Vector3 MakeDecision(Vector3 currentPosition)
    {
        // Implement your decision-making logic here
        // Example: AI always moves to the right and chooses randomly between parallel above or below
        return new Vector3(currentPosition.x + 1, currentPosition.y + Random.Range(-1, 2), currentPosition.z);
    }

    // Example usage
    void Start()
    {
        List<Vector3> path = FindPath(startPosition, targetPosition);
        if (path.Count > 0)
        {
            foreach (Vector3 pos in path)
            {
                Debug.Log("Next Position: " + pos);
            }
        }
        else
        {
            Debug.Log("No path found!");
        }
    }
}
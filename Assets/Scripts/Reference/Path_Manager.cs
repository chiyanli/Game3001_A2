using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.Networking;

public class PathNode // Graph nodes for pathfinding
{
    public GameObject Tile { get; private set; } //Associated Tile for node
    public List<PathConnection> connections;

    public PathNode(GameObject tile)
    {
        this.Tile = tile;
        this.connections = new List<PathConnection>();
    }
    public void AddConnection(PathConnection c)
    {
        connections.Add(c);
    }
    public void SetStatus(TileStatus status)
    {
        // Implémentez cette méthode selon votre logique de jeu
    }


}

[System.Serializable]

public class PathConnection
{
    public float Cost { get; set; }// Cost from tile to tile. Distance
    public PathNode FromNode { get; private set; }
    public PathNode ToNode { get; private set; }
    public PathConnection(PathNode from, PathNode to, float cost = 1f)
    {
        this.FromNode = from;
        this.ToNode = to;
        this.Cost = cost;
    }
}
public class NodeRecord // Utility class to document the pathfinding process.
{
    public PathNode Node { get; set; }
    public NodeRecord FromRecord { get; set; }
    public PathConnection pathConnection { get; set; }
    public float CostSoFar { get; set; }
    public NodeRecord(PathNode node = null)
    {
        this.Node = node;
        this.pathConnection = null;
        this.FromRecord = null;
        this.CostSoFar = 0f;
    }
}

[System.Serializable]


public class Path_Manager : MonoBehaviour
{
    public List<NodeRecord> openList;
    public List<NodeRecord> closedList;
    public List<PathConnection> path; // Ultimante path from start to goal.
    public static Path_Manager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        openList = new List<NodeRecord>();
        closedList = new List<NodeRecord>();
        path = new List<PathConnection>();
    }

    public void GetShortestPath(PathNode startNode, PathNode goalNode) // Dijksta's algorithm
    {

        if (path.Count > 0)
        {
            Initialize();
        }

        // Define a new node record for the current node
        NodeRecord currentRecord = null;

        // Add the start node to the open list
        openList.Add(new NodeRecord(startNode));

        // Iterate through processing each node
        while (openList.Count > 0)
        {
            // Find the smallest element in the open list
            currentRecord = GetSmallestNode();

            // If it is the goal node, then terminate
            if (currentRecord.Node == goalNode)
            {
                // Clean up lists and set status of current node to closed
                openList.Remove(currentRecord);
                closedList.Add(currentRecord);
                currentRecord.Node.SetStatus(TileStatus.CLOSED);
                break;
            }
            // Otherwise get its outgoing connections
            List<PathConnection> connections = currentRecord.Node.connections;

            // Loop through each connection in turn
            foreach (PathConnection connection in connections)
            {
                // Define an end node record
                NodeRecord endNodeRecord = null;

                // Get the cost estimate for the end node
                PathNode endNode = connection.ToNode;

                // Get the cost so far to the end node, and we define it here because we use it later
                float endNodeCost;
                endNodeCost = currentRecord.CostSoFar + connection.Cost;

                // Skip if the node is in the closed list
                if (closedList.Contains(endNodeRecord))
                {
                    continue;
                }

                // Or if it is in the open list
                else if (openList.Contains(endNodeRecord))
                {
                    // Find the end node record in the open list
                    endNodeRecord = openList.Find(record => record.Node == endNode);

                    // Our currently explored record is a worse route, so continue on
                    if (endNodeRecord.CostSoFar <= endNodeCost)
                    {
                        continue;
                    }
                }

                // Else we’ve got an unvisited node, so make a new record for it
                else
                {
                    endNodeRecord = new NodeRecord();
                    endNodeRecord.Node = endNode;
                }

                // We need to update the end node record
                endNodeRecord.CostSoFar = endNodeCost;
                endNodeRecord.pathConnection = connection;
                endNodeRecord.FromRecord = currentRecord;

                // And potentially add it to the open list
                if (!openList.Contains(endNodeRecord))
                {
                    openList.Add(endNodeRecord);
                    endNodeRecord.Node.SetStatus(TileStatus.OPEN);
                }
            }

            // We’ve finished looking at the connections for
            // the current node, so add it to the closed list
            // and remove it from the open list
            openList.Remove(currentRecord);
            closedList.Add(currentRecord);
            currentRecord.Node.SetStatus(TileStatus.CLOSED);
        }

        // We’re here if we’ve either found the goal, or
        // if we’ve no more nodes to search, so determine which
        if (currentRecord == null)
        {
            return; // If there is no currentRecord, best to early exit
        }
        if (currentRecord.Node != goalNode)
        {
            Debug.LogError("Could not find path to goal!");
        }
        else
        {
            Debug.Log("Found path to goal!");

            // Compile the list of connections in the path
            // Work back along the path, accumulating connections
            while (currentRecord.Node != startNode)
            {
                path.Add(currentRecord.pathConnection);
                currentRecord.Node.Tile.GetComponent<TileScript>().SetStatus(TileStatus.PATH);
                currentRecord = currentRecord.FromRecord;
            }

            // Reverse the path
            path.Reverse();
        }

        // A little cleaning up
        openList.Clear();
        closedList.Clear();



    }
    public NodeRecord GetSmallestNode()
    {
        NodeRecord smallestNode = openList[0];
        foreach (NodeRecord node in openList)
        {// Should use the old for and start with second node in list
            if (node.CostSoFar < smallestNode.CostSoFar)
            {
                smallestNode = node;
            }
            else if (node.CostSoFar == smallestNode.CostSoFar)
            {
                smallestNode = (Random.value < 0.5 ? node : smallestNode);
            }

        }
        return smallestNode;
    }
    public bool ConstainsNode(List<NodeRecord> list, PathNode node)
    {
        foreach (NodeRecord record in list)
        {
            if (record.Node == node) return true; // Early exit.
        }
        return false;
    }
    public NodeRecord GetNodeRecord(List<NodeRecord> list, PathNode node)
    {
        foreach (NodeRecord record in list)
        {
            if (record.Node == node) return record; // Early exit.
        }
        return null;
    }
}

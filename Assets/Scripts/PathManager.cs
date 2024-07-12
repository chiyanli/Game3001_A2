//using UnityEngine;
//using System.Collections.Generic;
//using System.Linq;

//public class PathManager : MonoBehaviour
//{
//    public PathNode[,] Nodes;
//    public Grid Grid;

//    public void InitializeNodes(int width, int height)
//    {
//        Nodes = new PathNode[width, height];
//        Grid = GetComponent<Grid>();
//        for (int x = 0; x < width; x++)
//        {
//            for (int y = 0; y < height; y++)
//            {
//                Nodes[x, y] = new GameObject($"Node_{x}_{y}").AddComponent<PathNode>();
//                Nodes[x, y].Position = new Vector2Int(x, y);
//            }
//        }
//    }

//    public List<PathNode> FindPath(Vector3 startPosition, Vector3Int targetPosition)
//    {
//        PathNode startNode = GetNodeFromWorldPosition(startPosition);
//        PathNode targetNode = Nodes[targetPosition.x, targetPosition.y];

//        var openSet = new SortedSet<PathNode>(new PathNodeComparer());
//        var closedSet = new HashSet<PathNode>();

//        startNode.Cost = 0;
//        openSet.Add(startNode);

//        while (openSet.Count > 0)
//        {
//            PathNode currentNode = openSet.First();
//            openSet.Remove(currentNode);

//            if (currentNode == targetNode)
//            {
//                return ReconstructPath(startNode, targetNode);
//            }

//            closedSet.Add(currentNode);

//            foreach (var connection in currentNode.Connections)
//            {
//                PathNode neighbor = connection.ToNode;
//                if (closedSet.Contains(neighbor))
//                {
//                    continue;
//                }

//                float newCost = currentNode.Cost + connection.Cost;
//                if (newCost < neighbor.Cost)
//                {
//                    neighbor.Cost = newCost;
//                    neighbor.PreviousNode = currentNode;

//                    if (!openSet.Contains(neighbor))
//                    {
//                        openSet.Add(neighbor);
//                    }
//                }
//            }
//        }

//        return new List<PathNode>(); // Return an empty path if no path is found
//    }

//    private PathNode GetNodeFromWorldPosition(Vector3 worldPosition)
//    {
//        Vector3Int cellPosition = Grid.WorldToCell(worldPosition);
//        return Nodes[cellPosition.x, cellPosition.y];
//    }

//    private List<PathNode> ReconstructPath(PathNode startNode, PathNode endNode)
//    {
//        var path = new List<PathNode>();
//        var currentNode = endNode;

//        while (currentNode != startNode)
//        {
//            path.Add(currentNode);
//            currentNode = currentNode.PreviousNode;
//        }
//        path.Add(startNode);
//        path.Reverse();

//        return path;
//    }
//}

//public class PathNodeComparer : IComparer<PathNode>
//{
//    public int Compare(PathNode x, PathNode y)
//    {
//        if (x.Cost < y.Cost)
//        {
//            return -1;
//        }
//        if (x.Cost > y.Cost)
//        {
//            return 1;
//        }
//        return 0;
//    }
//}

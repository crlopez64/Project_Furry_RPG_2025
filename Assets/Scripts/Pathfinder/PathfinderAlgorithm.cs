using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of creating the algorithm for a Unit to follow an optimal path.
/// </summary>
[RequireComponent (typeof(PathfinderGrid))]
[RequireComponent(typeof(PathfinderRequestManager))]
public class PathfinderAlgorithm : MonoBehaviour
{
    private PathfinderRequestManager pathfinderRequestManager;
    private PathfinderGrid grid;

    private void Awake()
    {
        pathfinderRequestManager = GetComponent<PathfinderRequestManager>();
        grid = GetComponent<PathfinderGrid>();
    }

    /// <summary>
    /// Start to find a path to follow.
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="targetPosition"></param>
    public void StartFindPath(Vector3 startPosition, Vector3 targetPosition, bool useMovementPenalty)
    {
        StartCoroutine(FindPath(startPosition, targetPosition, useMovementPenalty));
    }

    /// <summary>
    /// Find an optimal path between a starting position toward a target position.
    /// </summary>
    /// <param name="startingPosition"></param>
    /// <param name="targetPosition"></param>
    private IEnumerator FindPath(Vector3 startingPosition, Vector3 targetPosition, bool useMovementPenalty)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;
        PathfinderNode startNode = grid.NodeFromWorldPoint(startingPosition);
        PathfinderNode targetNode = grid.NodeFromWorldPoint(targetPosition);
        startNode.SetNewGCost(0);

        if (startNode.IsWalkable() && targetNode.IsWalkable())
        {
            Heap<PathfinderNode> openSet = new Heap<PathfinderNode>(grid.MaxSize);
            HashSet<PathfinderNode> closedSet = new HashSet<PathfinderNode>();
            openSet.Add(startNode);
            while (openSet.Count > 0)
            {
                PathfinderNode currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);
                if (currentNode == targetNode)
                {
                    pathSuccess = true;

                    break;
                }
                foreach (PathfinderNode neighbor in grid.GetNeighbors(currentNode))
                {
                    if ((!neighbor.IsWalkable()) || closedSet.Contains(neighbor))
                    {
                        continue;
                    }
                    int movementPenalty = useMovementPenalty ? neighbor.GetMovementPenalty() : 0;
                    int newMovementCostToNeighbor = currentNode.GetGCost() + GetDistance(currentNode, neighbor) + movementPenalty;
                    if (newMovementCostToNeighbor < neighbor.GetGCost() || (!openSet.Contains(neighbor)))
                    {
                        neighbor.SetNewGCost(newMovementCostToNeighbor);
                        neighbor.SetNewHCost(GetDistance(neighbor, targetNode));
                        neighbor.SetParentNode(currentNode);
                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbor);
                        }
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
            pathSuccess = waypoints.Length > 0;
        }
        pathfinderRequestManager.FinishProcessingPath(waypoints, pathSuccess);
    }

    /// <summary>
    /// Retrace the path between a given start node and end node.
    /// </summary>
    /// <param name="startNode"></param>
    /// <param name="endNode"></param>
    private Vector3[] RetracePath(PathfinderNode startNode, PathfinderNode endNode)
    {
        List<PathfinderNode> path = new List<PathfinderNode>();
        PathfinderNode currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.GetParent();
        }
        path.Add(startNode);
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    /// <summary>
    /// Simplify a path to only give waypoints when a direction changes.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private Vector3[] SimplifyPath(List<PathfinderNode> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;
        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i-1].GetGridX() - path[i].GetGridX(), path[i-1].GetGridY()  - path[i].GetGridY());
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i-1].GetWorldPosition());
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    /// <summary>
    /// Return the distance between two nodes.
    /// </summary>
    /// <param name="nodeA"></param>
    /// <param name="nodeB"></param>
    /// <returns></returns>
    private int GetDistance(PathfinderNode nodeA, PathfinderNode nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.GetGridX() - nodeB.GetGridX());
        int distanceY = Mathf.Abs(nodeA.GetGridY() - nodeB.GetGridY());

        if (distanceX > distanceY)
        {
            return (14 * distanceY) + (10 * (distanceX - distanceY));
        }
        return (14 * distanceX) + (10 * (distanceY - distanceX));
    }
}

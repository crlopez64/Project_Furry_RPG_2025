using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The grid for Pathfinding
/// </summary>
[RequireComponent(typeof(PathfinderRequestManager))]
public class PathfinderGrid : MonoBehaviour
{
    private Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();
    /// <summary>
    /// The entire layermask composed of all the Unity Layers of what a Unit can walk on.
    /// </summary>
    private LayerMask walkableMask;

    /// <summary>
    /// The array of Nodes that makes up the grid.
    /// </summary>
    private PathfinderNode[,] grid;

    /// <summary>
    /// Full length of an individual node.
    /// </summary>
    private float nodeDiameter;

    /// <summary>
    /// Total Nodes along the X axis.
    /// </summary>
    private uint gridSizeLength;

    /// <summary>
    /// Total Nodes along the Y axis.
    /// </summary>
    private uint gridSizeWidth;

    /// <summary>
    /// Part of the map that cannot be used for pathfinding.
    /// </summary>
    public LayerMask unwalkableMask;

    /// <summary>
    /// Size of Grid.
    /// </summary>
    public Vector2 gridWorldSize;

    /// <summary>
    /// The half-length of an individual node. Node will always be square.
    /// </summary>
    public float nodeRadius;

    /// <summary>
    /// The group of regions a Unit can walk on, along with a penalty value to avoid certain terrain.
    /// </summary>
    public TerrainType[] walkableRegions;

    public bool displayGridGizmos;


    private void Awake()
    {
        CreateGrid();
    }
    private void OnDrawGizmos()
    {
        // Draw the pathfinding grid
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 0f));

        if ((grid != null) && displayGridGizmos)
        {
            foreach (PathfinderNode node in grid)
            {
                Gizmos.color = node.IsWalkable() ? new Color(1f, 1f, 1f, 0.1f) : new Color(1f, 0f, 0f, 0.2f);
                Gizmos.DrawWireCube(node.GetWorldPosition(), Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }

    public int MaxSize
    {
        get { return (int)(gridSizeLength * gridSizeWidth); }
    }

    /// <summary>
    /// Get a specific node from the grid based on a world position.
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public PathfinderNode NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = ((worldPosition.x - transform.position.x) / gridWorldSize.x) + 0.5f;
        float percentY = ((worldPosition.y - transform.position.y) / gridWorldSize.y) + 0.5f;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x = Mathf.RoundToInt((gridSizeLength - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeWidth - 1) * percentY);
        return grid[x, y];
    }

    /// <summary>
    /// Return a List of a neighbors for any given Pathfinder Node.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public List<PathfinderNode> GetNeighbors(PathfinderNode node)
    {
        List<PathfinderNode> neighbors = new List<PathfinderNode>();
        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                //Skip Center node, aka the parameter node
                if ((x == 0) && (y == 0))
                {
                    continue;
                }
                int checkX = node.GetGridX() + x;
                int checkY = node.GetGridY() + y;

                if ((checkX >= 0)
                    && (checkX < gridSizeLength)
                    && (checkY >= 0)
                    && (checkY < gridSizeWidth))
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbors;
    }

    /// <summary>
    /// Prepare the Node Diameter, and proper perimeter lengths of the Grid.
    /// </summary>
    private void PrepareGrid()
    {
        nodeDiameter = nodeRadius * 2f;
        gridSizeLength = (uint)Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeWidth = (uint)Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
    }

    /// <summary>
    /// Create the Grid and prepare if each node should be walked on or not.
    /// </summary>
    private void CreateGrid()
    {
        PrepareGrid();
        foreach(TerrainType region in walkableRegions)
        {
            walkableMask.value |= region.terrainMask.value;
            walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
        }
        grid = new PathfinderNode[gridSizeLength, gridSizeWidth];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.up * gridWorldSize.y/2;
        for(int x = 0; x < gridSizeLength; x++)
        {
            for(int y = 0; y < gridSizeWidth; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask);
                int movementPenalty = 0;
                if (walkable)
                {
                    RaycastHit2D hit = Physics2D.Raycast(worldPoint + Vector3.up * 50, Vector2.down, 100f, walkableMask);
                    if (hit)
                    {
                        walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                    }
                }
                grid[x, y] = new PathfinderNode(walkable, worldPoint, x, y, movementPenalty);
            }
        }
    }

    [System.Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }
}

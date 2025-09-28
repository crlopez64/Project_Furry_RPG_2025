using UnityEngine;

/// <summary>
/// Creates a Node to be used in a pathfinding map.
/// </summary>
public class PathfinderNode : IHeapItem<PathfinderNode>
{
    private PathfinderNode parentNode;
    private Vector2 worldPosition;
    private readonly bool walkable;
    private int movementPenalty;
    private int heapIndex;
    private int gridX;
    private int gridY;
    private int gCost;
    private int hCost;

    public PathfinderNode(bool walkable, Vector2 worldPosition, int gridX, int gridY, int movementPenalty)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
        this.movementPenalty = movementPenalty;
    }

    /// <summary>
    /// Set parent node.
    /// </summary>
    /// <param name="parent"></param>
    public void SetParentNode(PathfinderNode parent)
    {
        this.parentNode = parent;
    }
    /// <summary>
    /// Update this node's new distance away from a Starting Node.
    /// </summary>
    /// <param name="incomingGCost"></param>
    public void SetNewGCost(int incomingGCost)
    {
        gCost = incomingGCost;
    }

    /// <summary>
    /// Update this node's new distance away from a Target Node.
    /// </summary>
    /// <param name="incomingHCost"></param>
    public void SetNewHCost(int incomingHCost)
    {
        hCost = incomingHCost;
    }

    /// <summary>
    /// Return if this Node is walkable.
    /// </summary>
    /// <returns></returns>
    public bool IsWalkable()
    {
        return walkable;
    }

    /// <summary>
    /// Get the World Position of this Node.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetWorldPosition()
    {
        return worldPosition;
    }

    /// <summary>
    /// Return movement penalty.
    /// </summary>
    /// <returns></returns>
    public int GetMovementPenalty()
    {
        return movementPenalty;
    }

    /// <summary>
    /// Return the X position relative to the grid this Node is in.
    /// </summary>
    /// <returns></returns>
    public int GetGridX()
    {
        return gridX;
    }

    /// <summary>
    /// Return the Y position relative to the grid this Node is in.
    /// </summary>
    /// <returns></returns>
    public int GetGridY()
    {
        return gridY;
    }

    /// <summary>
    /// Return the distance away from a Starting Node plus distance away from the Target node.
    /// </summary>
    /// <returns></returns>
    public int GetFCost()
    {
        return gCost + hCost;
    }
    /// <summary>
    /// Return the distance away from a Starting Node.
    /// </summary>
    /// <returns></returns>
    public int GetGCost()
    {
        return gCost;
    }
    /// <summary>
    /// Return the distance away from a Target node.
    /// </summary>
    /// <returns></returns>
    public int GetHCost()
    {
        return hCost;
    }

    /// <summary>
    /// Return the parent node.
    /// </summary>
    /// <returns></returns>
    public PathfinderNode GetParent()
    {
        return parentNode;
    }

    public int HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }

    public int CompareTo(PathfinderNode other)
    {
        int compare = GetFCost().CompareTo(other.GetFCost());
        if (compare == 0)
        {
            compare = GetHCost().CompareTo(other.GetHCost());
        }
        return -compare;
    }

    public int CompareTo(object obj)
    {
        if (obj.GetType() == typeof(PathfinderNode))
        {
            return CompareTo(obj as PathfinderNode);
        }
        throw new System.NotImplementedException();
    }
}

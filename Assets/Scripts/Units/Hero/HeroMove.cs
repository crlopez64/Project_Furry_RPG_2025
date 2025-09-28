using UnityEngine;

/// <summary>
/// Movement class for the Protagonist group.
/// </summary>
public class HeroMove : UnitMove
{
    protected byte orderInLine;

    public override void Start()
    {
        base.Start();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private Vector2 GetFollowInLineTrueLocation(UnitMove unitMove)
    {
        switch(unitMove.GetFacingDirection())
        {
            case Direction.RIGHT:
                return (Vector2)unitMove.transform.position - new Vector2(-2, 0);
            case Direction.UP_RIGHT:
                return (Vector2)unitMove.transform.position - new Vector2(-2, -2);
            case Direction.UP:
                return (Vector2)unitMove.transform.position - new Vector2(0, -2);
            case Direction.UP_LEFT:
                return (Vector2)unitMove.transform.position - new Vector2(2, -2);
            case Direction.LEFT:
                return (Vector2)unitMove.transform.position - new Vector2(2, 0);
            case Direction.DOWN_LEFT:
                return (Vector2)unitMove.transform.position - new Vector2(2, 2);
            case Direction.DOWN:
                return (Vector2)unitMove.transform.position - new Vector2(0, 2);
            case Direction.DOWN_RIGHT:
                return (Vector2)unitMove.transform.position - new Vector2(-2, 2);
            default:
                return (Vector2)unitMove.transform.position - new Vector2(-2, 0);
        }
    }

    /// <summary>
    /// Set the Order in Line for this unit. Value starts at 1; 0=Unit should not be set.
    /// </summary>
    /// <param name="orderInLine"></param>
    public void SetOrderInLine(byte orderInLine)
    {
        this.orderInLine = orderInLine;
    }

    /// <summary>
    /// Return if this Hero is in line.
    /// </summary>
    /// <returns></returns>
    public bool InLine()
    {
        return (orderInLine > 0) && (orderInLine <= 4);
    }

    /// <summary>
    /// Get this Hero's order in line.
    /// </summary>
    /// <returns></returns>
    public byte GetOrderInLine()
    {
        return orderInLine;
    }
}

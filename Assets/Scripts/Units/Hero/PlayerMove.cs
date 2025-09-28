using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMove : HeroMove
{
    private InputAction overworldMove;
    private bool playerCanMove;

    /**
     * NOTES:
     * 
     * context.start     = ??
     * context.performed = On Button Down
     * context.canceled  = On Button Up
     * 
     * 
     * 
     * 
     */

    public override void Awake()
    {
        base.Awake();
        overworldMove = InputSystem.actions.FindAction("Move");
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Vector2 getInput = overworldMove.ReadValue<Vector2>();
        InputMove(getInput);
    }

    /// <summary>
    /// Add Input move to this unit.
    /// </summary>
    /// <param name="context"></param>
    public void InputMove(Vector2 getInput)
    {
        if (!playerCanMove)
        {
            if (followFrequency != FollowFrequency.NONE)
            {
                inputVelocity = Vector2.zero;
            }
            return;
        }
        inputVelocity = getInput;
        if (getInput != Vector2.zero)
        {
            facingDirection = (Direction)GetEightSectionVector(getInput);
        }
    }

    /// <summary>
    /// Stop the player from letting them move the unit.
    /// </summary>
    public void StopPlayerMove()
    {
        playerCanMove = false;
        inputVelocity = Vector2.zero;
    }

    /// <summary>
    /// Let Player move the unit.
    /// </summary>
    public void StartPlayerMove() 
    {
        playerCanMove = true;
        followFrequency = FollowFrequency.NONE;
    }

}

using System;
using System.Collections;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

/// <summary>
/// Script in charge of Unit movement.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class UnitMove : MonoBehaviour
{
    private BattleManager battleManager;
    protected static readonly float withinDistanceAbsoluteLocation = 0.5f;
    protected static readonly float withinDistanceGiveSpace = 3f;
    protected Transform toFollow;
    protected Animator animator;
    protected Rigidbody2D rb2D;
    protected StateForBattleManager stateForBattleManager;
    protected FollowFrequency followFrequency;
    protected Direction facingDirection;
    protected Vector3[] pathToFollow;
    protected Vector2 inputVelocity;
    protected Vector2 destination;
    protected int pathTargetIndex;
    protected readonly float battleMoveSpeed = 13f;
    protected readonly float overworldPartySpeed = 6f;
    protected readonly float battleKnockbackSpeedSlow = 6f;
    protected readonly float battleKnockbackSpeedFast = 10f;
    protected float withinDistanceRange = 0f;
    protected float autoMoveSpeed = 10f;

    /// <summary>
    /// What should BattleManager do when Unit arrives to its destination?
    /// </summary>
    public enum StateForBattleManager : byte
    {
        NONE,
        ATTACK,
        END_TURN
    }

    /// <summary>
    /// The 8-way direction. Ordered in such a way that Player Input can easily take this in.
    /// </summary>
    public enum Direction : byte
    {
        RIGHT,
        UP_RIGHT,
        UP,
        UP_LEFT,
        LEFT,
        DOWN_LEFT,
        DOWN,
        DOWN_RIGHT
    }

    /// <summary>
    /// How should the unit move?
    /// </summary>
    protected enum FollowFrequency : byte
    {
        /// <summary>
        /// Do not move; Move directly with User Input.
        /// </summary>
        NONE,
        /// <summary>
        /// Move in a linear path directly toward a position.
        /// </summary>
        DIRECT,
        /// <summary>
        /// Idle and wait for a path to be provided by a pathfinder.
        /// </summary>
        PATHFINDER_IDLE,
        /// <summary>
        /// Follow a set path via a pathfinder.
        /// </summary>
        PATHFINDER_WALK,
    }

    /// <summary>
    /// Move speed for units.
    /// </summary>
    protected enum MoveSpeed : byte
    {
        OVERWORLD,
        BATTLE_SPEED,
        BATTLE_KNOCKBACK_SLOW,
        BATTLE_KNOCKBACK_FAST
    }

    public virtual void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }
    public virtual void Start()
    {
        inputVelocity = Vector2.zero;
        facingDirection = Direction.DOWN;
        withinDistanceRange = withinDistanceAbsoluteLocation;
        if (toFollow != null)
        {
            followFrequency = FollowFrequency.PATHFINDER_WALK;
            withinDistanceRange = withinDistanceGiveSpace;
            PathfinderRequestManager.RequestPath(transform.position, toFollow.position, false, OnPathFound);
        }
    }
    public virtual void FixedUpdate()
    {
        switch(followFrequency)
        {
            case FollowFrequency.NONE:
                Move(inputVelocity, MoveSpeed.OVERWORLD, true);
                break;
            case FollowFrequency.DIRECT:
                if (WithinDistanceTo(destination))
                {
                    followFrequency = FollowFrequency.NONE;
                    //Battle Logic
                    if (battleManager == null)
                    {
                        break;
                    }
                    switch (stateForBattleManager)
                    {
                        case StateForBattleManager.NONE:
                            break;
                        case StateForBattleManager.ATTACK:
                            battleManager.ExecuteTurnPrepareAttack();
                            break;
                        case StateForBattleManager.END_TURN:
                            battleManager.EndCurrentTurn();
                            break;
                    }
                    break;
                }
                Move(destination - (Vector2)transform.position, MoveSpeed.BATTLE_SPEED, false);
                break;
            case FollowFrequency.PATHFINDER_IDLE:
                Move(Vector2.zero, MoveSpeed.OVERWORLD, true);
                if (!WithinDistanceTo(toFollow))
                {
                    followFrequency = FollowFrequency.PATHFINDER_WALK;
                    BeginPathfindingAgain();

                }
                break;
            case FollowFrequency.PATHFINDER_WALK:
                if (!HasTargetToFollow())
                {
                    followFrequency = FollowFrequency.PATHFINDER_IDLE;
                    break;
                }
                if (WithinDistanceTo(toFollow))
                {
                    ClearPathToFind();
                }
                else
                {
                    //TODO: If the Unit is stuck, clear the path and begin pathfinding again.
                    //if (rb2D.linearVelocity.magnitude <= 0.1f)
                    //{
                    //    ClearPathToFind();
                    //    BeginPathfindingAgain();
                    //    break;
                    //}
                    if (ReachedPathProvidedByPathfinder())
                    {
                        PathfinderRequestManager.RequestPath(transform.position, toFollow.position, false, OnPathFound);
                    }
                }
                break;
        }

        //Animator Things
        animator.SetInteger("OverworldFacingDirection", (int)facingDirection);
        animator.SetFloat("Velocity", rb2D.linearVelocity.magnitude);
        animator.SetBool("Battle_MovingTowardFacingDirection", BattleMovingTowardFacingDirection());
    }
    public void OnDrawGizmos()
    {
        if (pathToFollow != null)
        {
            for(int i = pathTargetIndex; i < pathToFollow.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(pathToFollow[i], Vector3.one);
                if (i == pathTargetIndex)
                {
                    Gizmos.DrawLine(transform.position, pathToFollow[i]);
                }
                else
                {
                    Gizmos.DrawLine(pathToFollow[i - 1], pathToFollow[i]);
                }
            }
        }
    }

    /// <summary>
    /// Have the unit begin to move toward its given path.
    /// </summary>
    /// <param name="newPath"></param>
    /// <param name="pathSuccessful"></param>
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            pathToFollow = newPath;
            StopCoroutine(FollowPath());
            StartCoroutine(FollowPath());
        }
    }

    /// <summary>
    /// Have the unit follow the path based on what the Pathfinder Algorithm has provided.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FollowPath()
    {
        // If no path is available or already within distance to the follow target, stop following
        if (pathToFollow == null || pathToFollow.Length == 0)
        {
            yield break;
        }
        Vector3 currentWaypoint = pathToFollow[0];
        pathTargetIndex = 0;
        while (true)
        {
            // If path gets cleared while following, exit and clear state
            if (pathToFollow == null || pathToFollow.Length == 0)
            {
                yield break;
            }
            if (Vector3.Distance(transform.position, currentWaypoint) < 0.125f)
            {
                pathTargetIndex++;
                if (pathTargetIndex >= pathToFollow.Length)
                {
                    yield break;
                }
                currentWaypoint = pathToFollow[pathTargetIndex];
            }
            Vector3 workingDirection = currentWaypoint - transform.position;
            Move(workingDirection, MoveSpeed.OVERWORLD, true);
            yield return null;
        }
    }

    /// <summary>
    /// Move the unit directly toward a position.
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="battleManager"></param>
    /// <param name="stateForBattleManager"></param>
    public void MoveUnitDirectlyToLocation(Vector2 destination, BattleManager battleManager, StateForBattleManager stateForBattleManager)
    {
        this.destination = destination;
        this.battleManager = battleManager;
        this.stateForBattleManager = stateForBattleManager;
        followFrequency = FollowFrequency.DIRECT;
    }

    /// <summary>
    /// Move the Unit.
    /// </summary>
    /// <param name="directionalInput"></param>
    /// <param name="moveSpeed"></param>
    /// <param name="eightDirectionOnly"></param>
    protected void Move(Vector2 directionalInput, MoveSpeed moveSpeed, bool eightDirectionOnly)
    {
        byte eighthAngle = GetEightSectionVector(directionalInput);
        facingDirection = (Direction)eighthAngle;
        float movingSpeed = 0;
        if (directionalInput != Vector2.zero)
        {
            switch (moveSpeed)
            {
                case MoveSpeed.OVERWORLD:
                    movingSpeed = overworldPartySpeed;
                    break;
                case MoveSpeed.BATTLE_SPEED:
                    movingSpeed = battleMoveSpeed;
                    break;
                case MoveSpeed.BATTLE_KNOCKBACK_SLOW:
                    movingSpeed = battleKnockbackSpeedSlow;
                    break;
                case MoveSpeed.BATTLE_KNOCKBACK_FAST:
                    movingSpeed = battleKnockbackSpeedFast;
                    break;
            }
        }
        rb2D.linearVelocity = (eightDirectionOnly ? GetDirectionalVelocity(eighthAngle) : directionalInput.normalized) * movingSpeed;
    }

    /// <summary>
    /// Return this Unit to follow its target.
    /// </summary>
    protected void BeginPathfindingAgain()
    {
        if ((toFollow == null) || (followFrequency != FollowFrequency.PATHFINDER_WALK))
        {
            return;
        }
        followFrequency = FollowFrequency.PATHFINDER_WALK;
        withinDistanceRange = withinDistanceGiveSpace;
        PathfinderRequestManager.RequestPath(transform.position, toFollow.position, false, OnPathFound);
    }

    /// <summary>
    /// Set the Subject to be following.
    /// </summary>
    /// <param name="unitMove"></param>
    public void SetToFollowInLine(UnitMove unitMove)
    {
        toFollow = unitMove.transform;
        autoMoveSpeed = overworldPartySpeed;
        followFrequency = FollowFrequency.PATHFINDER_WALK;
        withinDistanceRange = withinDistanceGiveSpace;
    }

    /// <summary>
    /// Return where this Unit is facing.
    /// </summary>
    /// <returns></returns>
    public Direction GetFacingDirection()
    {
        return facingDirection;
    }

    /// <summary>
    /// Return if this Unit is within distance of a world position.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    protected bool WithinDistanceTo(Vector2 position)
    {
        return Vector3.Distance(transform.position, position) <= withinDistanceRange;
    }

    /// <summary>
    /// Return if this Unit is within distance of its toFollow unit.
    /// Return true if toFollow does not exist.
    /// </summary>
    /// <param name="toFollow"></param>
    /// <returns></returns>
    protected bool WithinDistanceTo(Transform toFollow)
    {
        if (toFollow == null)
        {
            return true;
        }
        return Vector3.Distance(transform.position, toFollow.position) <= withinDistanceRange;
    }

    /// <summary>
    /// Return if this unit has a path to follow.
    /// </summary>
    /// <returns></returns>
    protected bool HasPathToFollow()
    {
        return pathToFollow != null;
    }

    /// <summary>
    /// Return if this Unit has a target to follow for Pathfinder.
    /// </summary>
    /// <returns></returns>
    protected bool HasTargetToFollow()
    {
        return toFollow != null;
    }

    /// <summary>
    /// Return if Unit has reached the path as provided by the Pathfinder.
    /// </summary>
    /// <returns></returns>
    protected bool ReachedPathProvidedByPathfinder()
    {
        if (pathToFollow == null)
        {
            return false;
        }
        return pathTargetIndex >= pathToFollow.Length;
    }

    /// <summary>
    /// Get an imperfect input or input velocity to a single byte.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    protected byte GetEightSectionVector(Vector2 input)
    {
        if (input == Vector2.zero)
        {
            return (byte)facingDirection;
        }
        float angle = Mathf.Atan2(input.y, input.x);
        return (byte)(Mathf.Round((8 * angle) / (2 * Mathf.PI) + 8) % 8);
    }

    /// <summary>
    /// Get the directional velocity for this Unit.
    /// </summary>
    /// <param name="eighthAngle"></param>
    /// <returns></returns>
    protected Vector2 GetDirectionalVelocity(byte eighthAngle)
    {
        switch (eighthAngle)
        {
            case 0: //Right
                return new Vector2(1f, 0f);
            case 1: //Up-Right
                return new Vector2(1f, 1f).normalized;
            case 2: //Up
                return new Vector2(0f, 1f);
            case 3: //Up-Left
                return new Vector2(-1f, 1f).normalized;
            case 4: //Left
                return new Vector2(-1f, 0f);
            case 5: //Down-Left
                return new Vector2(-1f, -1f).normalized;
            case 6: //Down
                return new Vector2(0f, -1f);
            case 7: //Down-Right
                return new Vector2(1f, -1f).normalized;
            default: //Neutral
                return Vector2.zero;
        }
    }

    /// <summary>
    /// Clean up pathfinding variables and stop following the path.
    /// </summary>
    private void ClearPathToFind()
    {
        StopCoroutine(FollowPath());
        followFrequency = FollowFrequency.PATHFINDER_IDLE;
        pathToFollow = null;
        pathTargetIndex = 0;
    }

    /// <summary>
    /// Should Unit move toward or backdash when moving during Battle Scene?
    /// </summary>
    /// <returns></returns>
    private bool BattleMovingTowardFacingDirection()
    {
        return (((int)facingDirection == 0) && (rb2D.linearVelocity.x >= 0f))
            || (((int)facingDirection == 4) && (rb2D.linearVelocity.x <= 0f));
    }
}

using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Windows;

/// <summary>
/// Script in charge of Unit movement.
/// </summary>
public class UnitMove : MonoBehaviour
{
    private BattleManager battleManager;
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
    protected readonly float overworldPartySpeed = 4f;
    protected readonly float battleMoveSpeed = 10f;
    protected readonly float inputSpeed = 5f;
    protected float autoMoveSpeed = 10f;

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
    /// How unit should move? Movement only via UserInput (also ties with zero move); move directly toward a target; move via pathway
    /// </summary>
    public enum FollowFrequency : byte
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
        /// Follow a set path via a pathfinder.
        /// </summary>
        PATHFINDER
    }

    /// <summary>
    /// What should BattleManager do when Unit arrives to its destination?
    /// </summary>
    public enum StateForBattleManager : byte
    {
        NONE,
        ATTACK,
        END_TURN
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
        if (toFollow != null)
        {
            PathfinderRequestManager.RequestPath(transform.position, toFollow.position, false, OnPathFound);
        }
    }
    public virtual void FixedUpdate()
    {
        switch(followFrequency)
        {
            case FollowFrequency.NONE:
                rb2D.linearVelocity = inputVelocity * inputSpeed;
                break;
            case FollowFrequency.DIRECT:
                float step = autoMoveSpeed * Time.deltaTime;
                Vector3 getMoveTowards = Vector3.MoveTowards(transform.position, destination, step);
                transform.position = getMoveTowards;
                facingDirection = (Direction)GetEightSectionVector(getMoveTowards);
                if (Vector3.Distance(transform.position, destination) < 0.01)
                {
                    Debug.Log("Reached destination!");
                    followFrequency = FollowFrequency.NONE;
                    if (battleManager == null)
                    {
                        break;
                    }
                    else
                    {
                        switch(stateForBattleManager)
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
                    }
                    
                }
                break;
            case FollowFrequency.PATHFINDER:
                //TODO: If follow player (or follow next unit), update destination Transform
                //If following player, and player looking away from unit, following in line
                //If following player, and player looking at unit, move away from Player (still facing at player) until Player is moving out
                if (!HasTargetToFollow())
                {
                    break;
                }
                if (ReachedPathProvidedByPathfinder())
                {
                    if (!WithinDistanceTo(toFollow)) //If not within 2u
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
    private void OnDrawGizmos()
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

    private void ClearPathToFind()
    {
        pathToFollow = null;
        pathTargetIndex = 0;
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
        Vector3 currentWaypoint = pathToFollow[0];
        pathTargetIndex = 0;
        while(true)
        {
            if (transform.position == currentWaypoint)
            {
                pathTargetIndex++;
                if (pathTargetIndex >= pathToFollow.Length)
                {
                    yield break;
                }
                currentWaypoint = pathToFollow[pathTargetIndex];
            }
            float step = autoMoveSpeed * Time.deltaTime;
            Vector3 getMoveTowards = Vector3.MoveTowards(transform.position, currentWaypoint, step);
            transform.position = getMoveTowards;
            yield return null;
        }
    }

    /// <summary>
    /// Move the unit directly toward a position.
    /// </summary>
    public void MoveUnitDirectlyToLocation(Vector2 destination, BattleManager battleManager, StateForBattleManager stateForBattleManager)
    {
        this.destination = destination;
        this.battleManager = battleManager;
        this.stateForBattleManager = stateForBattleManager;
        followFrequency = FollowFrequency.DIRECT;
    }

    /// <summary>
    /// Stop auto move and/or pathfinding.
    /// </summary>
    public void StopAutoMoveLocation()
    {
        followFrequency = FollowFrequency.NONE;
        destination = Vector2.zero;
    }

    /// <summary>
    /// Set the Subject to be following.
    /// </summary>
    /// <param name="unitMove"></param>
    public void SetToFollowInLine(UnitMove unitMove)
    {
        toFollow = unitMove.transform;
        autoMoveSpeed = overworldPartySpeed;
        followFrequency = FollowFrequency.PATHFINDER;
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
    /// Return if this Unit is within distance of its toFollow unit.
    /// Return true if toFollow does not exist.
    /// </summary>
    /// <returns></returns>
    protected bool WithinDistanceTo(Transform toFollow)
    {
        if (toFollow == null)
        {
            return true;
        }
        return Vector3.Distance(transform.position, toFollow.position) <= 1.85f;
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
    /// Should Unit move toward or backdash when moving during Battle Scene?
    /// </summary>
    /// <returns></returns>
    private bool BattleMovingTowardFacingDirection()
    {
        return (((int)facingDirection == 0) && (rb2D.linearVelocity.x >= 0f))
            || (((int)facingDirection == 4) && (rb2D.linearVelocity.x <= 0f));
    }
}

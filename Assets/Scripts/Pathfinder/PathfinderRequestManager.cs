using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

/// <summary>
/// Class for a Unit being able to request for a path to follow.
/// </summary>
[RequireComponent(typeof(PathfinderGrid))]
public class PathfinderRequestManager : MonoBehaviour
{
    private Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    private PathRequest currentPathRequest;
    private PathfinderAlgorithm pathfinding;
    private bool isProcessingPath;

    private static PathfinderRequestManager instance;

    private void Awake()
    {
        instance = this;
        pathfinding = GetComponent<PathfinderAlgorithm>();
    }

    /// <summary>
    /// Request a new path to find.
    /// </summary>
    /// <param name="pathStart"></param>
    /// <param name="pathEnd"></param>
    /// <param name="callback"></param>
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, bool useMovementPenalty, Action<Vector3[], bool> callback)
    {
        PathRequest pathRequest = new PathRequest(pathStart, pathEnd, useMovementPenalty, callback);
        instance.pathRequestQueue.Enqueue(pathRequest);
        instance.TryProcessNext();
    }
    
    /// <summary>
    /// Attempt the next path request to process.
    /// </summary>
    private void TryProcessNext()
    {
        if ((!isProcessingPath) && (pathRequestQueue.Count > 0))
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd, currentPathRequest.useMovementPenalty);
        }
    }

    /// <summary>
    /// Finish off processing a path and see if possible to run next path.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="success"></param>
    public void FinishProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public bool useMovementPenalty;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 pathStart, Vector3 pathEnd, bool useMovementPenalty, Action<Vector3[], bool> callback)
        {
            this.pathStart = pathStart;
            this.pathEnd = pathEnd;
            this.useMovementPenalty = useMovementPenalty;
            this.callback = callback;
        }
    }
}

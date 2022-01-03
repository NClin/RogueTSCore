using System.Collections;
using UnityEngine;
using Pathfinding;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(Seeker))]
public class MoveableUnit : MonoBehaviour
{

    private Vector3 previousStepVector;
    private Vector3 stepVector;
    private float t;
    private bool arrivedAtNextTile;
    private bool queuedMoveOrder = false;

    private float moveSpeed;
    public float MoveSpeed 
    { 
        get { return moveSpeed; }
        set 
        { 
            moveSpeed = value;
            if (moveSpeed < 0) moveSpeed = 0;
        }
    }

    // Pathfinding
    private Seeker seeker;
    private Path path;
    private int currentPathStep;
    private float arrivalThreshold = 0.001f;
    private Vector2Int occupiedTile;
    private bool isMoving;
    private MapOccupiedInfo mapOccupiedInfo;

    private bool initialized = false;

    // Stuck
    private bool stuck;
    private float stuckTime;
    private const float maxStuckTime = 0.5f;
    private float stuckCompletionDistanceThreshold = 2f; // Idea: If many units selected when move order given, increase this value on moved units to avoid infinite searching.
    private const float stuckCompletionDistanceThresholdBase = 2f;
    private const int maxStuckFrames = 5;
    private int currentStuckFrames = 0;
    private const float unstickerCheckInterval = 1;
    private bool needsUnsticking = false;
    private const int maxUnstickAttempts = 4;
    private int unstickAttempts = 0;
    private Coroutine MoveAlongPathCoroutine;

    // MoveTargetObject is an empty object to be instantiated as a transform to be used by the pathfinding system.
    [SerializeField]
    private Transform moveTargetObject;
    private Transform moveTarget;
    // Empty world object for organization.
    [SerializeField]
    private Transform moveTargetParent;





    void Start()
    {
        MoveSpeed = 5; // Testing, to be set by unit spawner.
    }

    IEnumerator ArrivedAtNextTileChecker()
    {
        arrivedAtNextTile = false;
        while (arrivedAtNextTile == false)
        {
            if (ArrivedAtVector3(stepVector))
            {
                arrivedAtNextTile = true;
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator Initialize()
    {
        yield return new WaitForEndOfFrame(); // Allows pathfinding graph scan to complete.

        // Initialize pathfinding
        moveTarget = Instantiate(moveTargetObject, moveTargetParent);
        moveTarget.name = this.name + " moveTarget";
        moveTarget.position = transform.position;
        seeker = GetComponent<Seeker>();

        mapOccupiedInfo = FindObjectOfType<MapOccupiedInfo>();
        TeleportTo(ClosestTileCoordinates(transform.position));
        stepVector = transform.position;
        previousStepVector = transform.position;
        SetPath(transform.position);
        OccupyClosestTile(transform.position);
        initialized = true;
    }

    void Update()
    {
        if (!initialized)
        {
            StartCoroutine(Initialize());
            return;
        }
        VerifyOccupiesTile();

    }

    public void MoveTo(Vector2 destination)
    {
        Debug.Log("hi");
        if (isMoving)
        {
            queuedMoveOrder = true;
        }
        StartCoroutine(MoveToCoroutine(destination));
    }

    public IEnumerator MoveToCoroutine(Vector2 destination)
    {
        // StartCoroutine(ArrivedAtNextTileChecker());

            while (queuedMoveOrder == true)
            {
                yield return null;
            }

        SetPath(destination);
    }

    private void VerifyOccupiesTile()
    {

        if (occupiedTile == null)
        {
            OccupyClosestTile(ClosestTileCoordinatesV3(transform.position));
        }
        mapOccupiedInfo.Occupy(occupiedTile);

        Debug.Log(mapOccupiedInfo.IsOccupied(occupiedTile));
    }

    private void StartMoving()
    {


        if (MoveAlongPathCoroutine != null)
        {
            StopCoroutine(MoveAlongPathCoroutine);
        }
        MoveAlongPathCoroutine = StartCoroutine(MoveAlongPath());
        isMoving = true;
    }

    private void StopMoving()
    {
        if (MoveAlongPathCoroutine != null)
        {
            StopCoroutine(MoveAlongPathCoroutine);
        }
        queuedMoveOrder = false;
        isMoving = false;
    }

    // This method will be called by the selection and orders system for player units, or by the AI controller for AI units.
    private void SetPath(Vector2 destination)
    {
        if (ArrivedAtVector3(destination))
        {
            OnPathComplete();
            return;
        }
        else
        {
            SetPathAvoidingOccupied(destination);
        }
    }

    private void SetMoveTarget(Vector2 destination)
    {
        moveTarget.transform.position = destination;
    }

    public void SetDirectPathToMoveTarget()
    {
        seeker.StartPath(transform.position, moveTarget.position, SetPathAndMove);
    }

    private void SetPathAvoidingOccupied(Vector2 destination)
    {
        SetMoveTarget(destination);
        // Callback sets path returned by seeker to current path.
        seeker.StartPath(transform.position, moveTarget.position, ProcessAndSetPathAvoidingOccupied);
    }


    private void ProcessAndSetPathAvoidingOccupied(Path p)
    {

        List<Vector2Int> blockedNodes = new List<Vector2Int>();

        foreach (Vector3 node in p.vectorPath)
        {
            var vec2 = ClosestTileCoordinates(node);
            if (mapOccupiedInfo.IsOccupied(vec2))
            {
                if (vec2 != occupiedTile)
                {
                    BlockNode(vec2);
                    blockedNodes.Add(vec2);
                }
            }
        }

        seeker.StartPath(transform.position, moveTarget.position, SetPathAndMove);

        if (blockedNodes.Count != 0)
        {
            foreach (Vector2Int blocked in blockedNodes)
            {
                UnblockNode(blocked);
            }
        }
    }

    private void OnPathComplete()
    {
        StopMoving();
    }

    public void SetPathAndMove(Path p)
    {
        if (p.error == false)
        {

            path = p;
            currentPathStep = 0;

            ////// hack
            //if (path.vectorPath[0] == path.vectorPath[1])
            //{
            //    path.vectorPath.RemoveAt(0);
            //}
            ////// hack end.
            /////


            StartMoving();
        }
        else { Debug.Log($"Path error: {p.error}"); }
    }

    private bool IsPathComplete()
    {
        if (currentPathStep >= path.vectorPath.Count) return true;
        else return false;
    }

    private bool ArrivedAtVector3(Vector3 vector)
    {
        if (Vector3.Distance(transform.position, vector) < arrivalThreshold)
        {
            return true;
        }
        else { return false; }
    }

    private IEnumerator MoveAlongPath()
    {
        while (!IsPathComplete())
        {

            if (!initialized)
            {
                yield break;
            }

            stepVector = path.vectorPath[currentPathStep];

            //arrived logic
            if (ArrivedAtVector3(stepVector))                                       // If we have arrived
            {
                t = 0;                                                              // we are now at a zero point in our lerp
                previousStepVector = stepVector;                                    // new is old

                if (queuedMoveOrder)
                {
                    queuedMoveOrder = false;
                    StopMoving();
                    yield break;
                }

                currentPathStep++;                                                  // time for next step

                if (IsPathComplete())                                                 // check if new step is final step
                {
                    OnPathComplete();
                    yield break;
                }

                stepVector = path.vectorPath[currentPathStep];                      // set that new step

            }


            //stuck logic
            if (mapOccupiedInfo.IsOccupied(ClosestTileCoordinates(stepVector))      // If next tile is occupied
                && occupiedTile != ClosestTileCoordinates(stepVector)               // And occupier is not this unit.
                && !CloseEnough(moveTarget.transform.position))                     // And we're not close enough to call it a day.
            {
                if (currentStuckFrames < maxStuckFrames)                            // If we haven't tried already the max times.
                {
                    Debug.Log("we stuck");
                    currentStuckFrames++;                                           // Count our tries
                    seeker.StartPath(ClosestTileCoordinatesV3(transform.position), moveTarget.position, ProcessAndSetPathAvoidingOccupied); // Recalculate path avoiding occupied
                    yield return null;                                              // and try again next frame.
                }
                else
                {
                    StopMoving();
                    yield break;
                }
            }

            OccupyClosestTile(stepVector);          // No problems, occupy next tile

            t += Time.deltaTime;                       // Update time
            transform.position = Vector3.Lerp(previousStepVector, stepVector, moveSpeed * t);           // Move.

            yield return null;
        }

    }

    void BlockNode(Vector2Int tileToAvoid)
    {
        Debug.Log($"blocking {tileToAvoid}");
        var tileVector3 = new Vector3(tileToAvoid.x, tileToAvoid.y, 0);
        var nodeToBlock = AstarPath.active.GetNearest(tileVector3).node;
        var guo = new GraphUpdateObject();
        guo.modifyWalkability = true;
        guo.setWalkability = false;
        guo.Apply(nodeToBlock);

    }

    bool CloseEnough(Vector3 destination)
    {
        if (path.vectorPath.Count < 5)
        {
            stuckCompletionDistanceThreshold = 1;
        }
        else
        {
            stuckCompletionDistanceThreshold = 3;
        }

        if (Vector3.Distance(transform.position, destination) < stuckCompletionDistanceThreshold)
        { 
            return true; 
        }    
        else 
        { 
            return false; 
        }
    }

    void UnblockNode(Vector2Int tileToAvoid)
    {
        //var bounds = new Bounds();
        //bounds.center = new Vector3(tileToAvoid.x, tileToAvoid.y, 0);
        //bounds.extents = new Vector3(0.5f, 0.5f, 0.4f);
        //var guo = new GraphUpdateObject(bounds);
        //guo.modifyWalkability = true;
        //guo.setWalkability = true;
        //AstarPath.active.UpdateGraphs(guo);

        var tileVector3 = new Vector3(tileToAvoid.x, tileToAvoid.y, 0);
        var nodeToBlock = AstarPath.active.GetNearest(tileVector3).node;
        var guo = new GraphUpdateObject();
        guo.modifyWalkability = true;
        guo.setWalkability = true;
        guo.Apply(nodeToBlock);
    }

    private void GetPathAvoidingTile(Vector2Int tileToAvoid)
    {
        BlockNode(tileToAvoid);
        SetDirectPathToMoveTarget();
        UnblockNode(tileToAvoid);
    }

    private void GetPathAvoidingAllOccupiedTiles()
    {
        var toAvoid = mapOccupiedInfo.GetAllBlockedTiles();

        foreach (Vector2Int blockedTile in toAvoid)
        {
            BlockNode(blockedTile);
        }
        
        SetDirectPathToMoveTarget();

        foreach (Vector2Int blockedTile in toAvoid)
        {
            UnblockNode(blockedTile);
        }
    }

    // this should be in a utility class.
    private Vector2Int ClosestTileCoordinates(Vector3 position)
    {
        return new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
    }

    private Vector3 ClosestTileCoordinatesV3(Vector3 position)
    {
        return new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), 0);
    }

    private void OccupyClosestTile(Vector3 mapTile)
    {
        var newTileVector = ClosestTileCoordinates(mapTile);

        if (occupiedTile != newTileVector) // Unoccupy previous tile.
        {
            mapOccupiedInfo.Deoccupy(occupiedTile);
        }

        occupiedTile = newTileVector;
        mapOccupiedInfo.Occupy(occupiedTile);
    }

    public void TeleportTo(Vector2Int targetTile)
    {
        var targetNode = AstarPath.active.data.gridGraph.GetNode(targetTile.x, targetTile.y);
        if (targetNode.Walkable && !mapOccupiedInfo.IsOccupied(targetTile))
        {
            var newPos = new Vector3(targetTile.x, targetTile.y, 0);
            gameObject.transform.position = newPos;
            OccupyClosestTile(newPos);
        }
    }

}

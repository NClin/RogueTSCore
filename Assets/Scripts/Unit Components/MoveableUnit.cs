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
    private const int maxStuckIterations = 5;
    private int stuckIterations = 0;
    private const float unstickerCheckInterval = 1;
    private bool needsUnsticking = false;
    private const int maxUnstickAttempts = 4;
    private int unstickAttempts = 0;
    private Coroutine unsticker;

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
        SetMoveTarget(transform.position);
        OccupyClosestTile(transform.position);
        initialized = true;
    }

    void Update()
    {
        if (!initialized)
        {
            StartCoroutine(Initialize());
        }

        VerifyOccupiesTile();
        if (isMoving)
        { TakeStepOnPath(); }
    }


    // This method will be called by the selection and orders system for player units, or by the AI controller for AI units.
    public void SetMoveTarget(Vector2 destination)
    {
        moveTarget.transform.position = destination;
        if(ArrivedAtVector3(destination))
        {
            return;
        }
        SetPathAvoidingObstacles();
    }

    private IEnumerator Unsticker()
    {
        SetPathAvoidingObstacles();
        if (stuck & unstickAttempts < maxUnstickAttempts)
        {
            stuckCompletionDistanceThreshold++;
            unstickAttempts++;
            yield return new WaitForSeconds(unstickerCheckInterval);
        }
        stuckCompletionDistanceThreshold = stuckCompletionDistanceThresholdBase;
        unstickAttempts = 0;
    }

    public void SetPathToMoveTarget()
    {
        seeker.StartPath(transform.position, moveTarget.position, MoveAlongPath);
    }

    public void SetPathAvoidingObstacles()
    {
        seeker.StartPath(transform.position, moveTarget.position, GetPathAvoidingOccupiedTest);
    }

    private void GetPathAvoidingOccupiedTest(Path p)
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

        seeker.StartPath(transform.position, moveTarget.position, MoveAlongPath);


        if (blockedNodes.Count != 0)
        {
            foreach (Vector2Int blocked in blockedNodes)
            {
                UnblockNode(blocked);
            }
        }
    }

    private IEnumerator GetPathAvoidingOccupied()
    {
        Debug.Log($"new coroutine");
        List<Vector2Int> blockedNodes = new List<Vector2Int>();

        if (path == null) SetPathToMoveTarget();

        foreach (Vector3 node in path.vectorPath)
        {
            var vec2 = ClosestTileCoordinates(node);
            if (mapOccupiedInfo.IsOccupied(vec2))
            {
                BlockNode(vec2);
                blockedNodes.Add(vec2);
            }
        }

        yield return seeker.StartPath(transform.position, moveTarget.position, MoveAlongPath);

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
        SetMoveTarget(transform.position);
        isMoving = false;
        stuck = false;

    }

    public void MoveAlongPath(Path p)
    {
        if (p.error == false)
        {
            path = p;
            currentPathStep = 0;
            isMoving = true;
        }
        else { Debug.Log($"Path error: {p.error}"); }
    }

    private bool PathComplete()
    {
        if (path == null) { return true; }
        if (currentPathStep > path.vectorPath.Count) { return true; }    // out of range somehow. Stop.
        if (currentPathStep == path.vectorPath.Count) { return true; } // path complete;
        else return false;
    }

    private bool ArrivedAtVector3(Vector3 vector)
    {
        if (Vector3.Distance(transform.position, vector) < arrivalThreshold)
        {
            t = 0;
            return true;
        }
        else { return false; }
    }

    private void BeginStuckTimer()
    {
        stuck = true;
        stuckTime = 0;
    }

    private void VerifyOccupiesTile()
    {
        if (occupiedTile == null)
        {
            OccupyClosestTile(transform.position);
        }
    }
    private void IncrementStuckTimer()
    {
        if (stuck)
        {
            stuckTime += Time.deltaTime;
        }
    }

    private void NoLongerStuck()
    {
        stuck = false;
        stuckTime = 0;
        stuckIterations = 0;
    }

    private void TakeStepOnPath()
    {
        if (PathComplete())
        {
            OnPathComplete(); 
            return;
        }

        if (stepVector == null)
        {
            previousStepVector = transform.position;
        }
        

        stepVector = path.vectorPath[currentPathStep];

        if (mapOccupiedInfo.IsOccupied(ClosestTileCoordinates(stepVector)))
        {
            if (occupiedTile != ClosestTileCoordinates(stepVector))
            {
                if (!stuck) BeginStuckTimer();
                if (CloseEnough(moveTarget.transform.position)) 
                {
                    stuck = false;
                    OnPathComplete(); 
                    return; 
                }
                if (stuckIterations < maxStuckIterations)
                {
                    if (stuck) IncrementStuckTimer();
                    if (stuckTime > maxStuckTime)
                    {
                        needsUnsticking = true;
                        if (unsticker == null)
                        {
                            Debug.Log("starting unstuck coroutine");
                            unsticker = StartCoroutine(Unsticker());
                        }
                        return;

                        //Debug.Log("I give up");
                        //SetMoveTarget(transform.position);
                        //stuckIterations = 0;
                        //OnPathComplete();
                        //return;
                    }
                    GetPathAvoidingAllOccupiedTiles();
                    stuckIterations++;
                    NoLongerStuck();
                    return;
                }
                return; 
            }
        }

        OccupyClosestTile(stepVector);

        //var dir = (stepVector - transform.position).normalized;           // depreciated.
        //transform.position += dir * moveSpeed * Time.deltaTime;

        t += Time.deltaTime;
        transform.position = Vector3.Lerp(previousStepVector, stepVector, moveSpeed * t);

        if (ArrivedAtVector3(stepVector))
        {
            previousStepVector = stepVector;
            currentPathStep++;
            if (PathComplete())
            {
                OnPathComplete(); return;
            }
            else
            {
                var nextStepVector = path.vectorPath[currentPathStep];
                if (!mapOccupiedInfo.IsOccupied(ClosestTileCoordinates(nextStepVector)))
                {
                    OccupyClosestTile(nextStepVector);
                }
            }
        }


    }

    void BlockNode(Vector2Int tileToAvoid)
    {
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
        SetPathToMoveTarget();
        UnblockNode(tileToAvoid);
    }

    private void GetPathAvoidingAllOccupiedTiles()
    {
        var toAvoid = mapOccupiedInfo.GetAllBlockedTiles();

        foreach (Vector2Int blockedTile in toAvoid)
        {
            BlockNode(blockedTile);
        }
        
        SetPathToMoveTarget();

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEditor;

public class Movement : MonoBehaviour
{
    public float moveSpeed;

    bool initialized = false;
    Coroutine initializer;

    MapOccupiedInfoSingleton _mapOccupiedInfo;
    bool stacks = false;

    Path path;
    float t;
    int maxAttempts = 10;
    int attempts = 0;
    bool stuck = false;

    float spamMoveToFrequency = 0.5f;
    float tSpamMoveTo = 0;
    int tSpamMoveToIterationsMax = 5;
    int tSpamMoveToIterations = 0;

    Vector2Int stepFrom;
    Vector2Int pathDestination;
    Vector2Int currentOccupied;
    int stepIndex;
    bool followPathCoroutine;
    bool stopOrder;

    bool shouldMove;




    void Update()
    {
        if (!initialized && initializer == null)
        {
            initializer = StartCoroutine(Initialize());
        }

        tSpamMoveTo += Time.deltaTime;
 
        if (Vector2.Distance(pathDestination, currentOccupied) > 0.5 
            && !shouldMove 
            && tSpamMoveTo > spamMoveToFrequency
            && tSpamMoveToIterations < tSpamMoveToIterationsMax)
        {
            MoveTo(new Vector3(pathDestination.x, pathDestination.y, 0));
            tSpamMoveTo = 0;
            tSpamMoveToIterations++; // when is this reset?
        }

        if (shouldMove)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else { GetComponent<SpriteRenderer>().color = Color.white; }

    }

    private void OnDrawGizmos()
    {
        Handles.Label(transform.position, pathDestination.x + ", " +  pathDestination.y);

        Gizmos.DrawLine(new Vector3(pathDestination.x, pathDestination.y, 0), transform.position);
    }

    public void StopOrder()
    {
        followPathCoroutine = false;
        stopOrder = true;
    }

    public void MoveTo(Vector3 input)
    {
        shouldMove = true;
        stopOrder = false;
        Debug.Log(stopOrder);
        stuck = false;
        pathDestination = new Vector2Int((int)ClosestTileCoordinatesV3(input).x, (int)ClosestTileCoordinatesV3(input).y);

        attempts = 0;

        if (!followPathCoroutine)
        {
            followPathCoroutine = true;
            StartCoroutine(FollowPathCoroutine());
        }
    }

    public Vector3 GetDestinationTile()
    {
        return new Vector3(pathDestination.x, pathDestination.y, 0);
    }

    /// <summary>
    /// I don't understand how this works and will have to rework it.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FollowPathCoroutine()
    {
        if (moveSpeed == 0
            || stopOrder == true)
        {
            shouldMove = false;
            Debug.Log("setting coroutine false. Should happen at coming to a complete halt");
            followPathCoroutine = false;
            yield return null;
        }

        var currentDestination = pathDestination;
        path = GetPathImmediately(transform.position, GetDestinationTile());
        stepIndex = 1;
        bool followingPath = true;


        while (!stopOrder)
        {
            // check destination. New path if changed.
            if (currentDestination != pathDestination)
            {
                currentDestination = pathDestination;
                path = GetPathImmediately(transform.position, GetDestinationTile());
                stepIndex = 1;
            }

            // check if arrived.
            if (stepIndex >= path.vectorPath.Count)
            {
                StopOrder();
                yield break;
            }

            if (followingPath)
            {
                stuck = false;

                // check if next step occupied
                Vector2Int nextStepV2Int = new Vector2Int((int)path.vectorPath[stepIndex].x, (int)path.vectorPath[stepIndex].y);

                if (_mapOccupiedInfo.IsOccupied(nextStepV2Int))
                {
                    attempts++;
                    if (attempts > maxAttempts) // doesn't currently work properly
                    {

                        StopOrder();
                        yield break;
                    }

                    //broke something here i think?

                    /// ###### THESE ARE BOTH GOOD OPTIONS FOR ASSNING PATH. 
                   

                    path = GetPathAvoidingTile(nextStepV2Int, currentDestination); // this is better for formation movement.
                    path = GetPathAvoidingAllOccupiedTiles(currentDestination); // this is better for swarm/flood movement.
                    stepIndex = 1;
                    stuck = true;
                    Debug.Log("attempts: " + attempts);

                    yield return new WaitForSeconds(0.1f);
                    continue;
                }


                if (!stuck) // redundant?
                {
                    // check if arrived (again, path may have changed)
                    if (stepIndex >= path.vectorPath.Count)
                    {

                        StopOrder();
                        yield break;
                    }

                    if (!stopOrder) // redundant?
                    {
                        yield return StartCoroutine(MoveFromToCell(stepFrom, path.vectorPath[stepIndex]));
                    }
                }
            }           
        }

        Debug.Log("setting coroutine false. Should only happen at coming to a complete halt");
        followPathCoroutine = false;

    }

    private IEnumerator Initialize()
    {
        yield return new WaitForEndOfFrame();

        t = 0;
        _mapOccupiedInfo = FindObjectOfType<MapOccupiedInfoSingleton>();
        if (_mapOccupiedInfo == null)
        {
            Debug.LogError("MapOccupiedInfo not found, critical failure.");
        }

        SnapToNearestTile();

        initialized = true;

    }

    private void SnapToNearestTile()
    {
        var closest = ClosestTileCoordinatesV3(transform.position);
        transform.position = closest;
        stepFrom = new Vector2Int ((int)closest.x, (int)closest.y);
        
        Occupy(stepFrom);
    }

    public void Die()
    {
        _mapOccupiedInfo.Deoccupy(currentOccupied);
    }

    private void Occupy(Vector2Int tile)
    {
        _mapOccupiedInfo.Occupy(tile);
        currentOccupied = tile;
    }

    private Vector3 ClosestTileCoordinatesV3(Vector3 position)
    {
        return new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), 0);
    }

    private void BlockNode(Vector2Int tileToAvoid)
    {
        var tileVector3 = new Vector3(tileToAvoid.x, tileToAvoid.y, 0);
        var nodeToBlock = AstarPath.active.GetNearest(tileVector3).node;
        var guo = new GraphUpdateObject();
        guo.modifyWalkability = true;
        guo.setWalkability = false;
        guo.Apply(nodeToBlock);
    }

    private void UnblockNode(Vector2Int tileToAvoid)
    {
        var tileVector3 = new Vector3(tileToAvoid.x, tileToAvoid.y, 0);
        var nodeToBlock = AstarPath.active.GetNearest(tileVector3).node;
        var guo = new GraphUpdateObject();
        guo.modifyWalkability = true;
        guo.setWalkability = true;
        guo.Apply(nodeToBlock);
    }


    private IEnumerator MoveFromToCell(Vector2 start, Vector2 end)
    {
        // quantize values
        start = ClosestTileCoordinatesV3(start);
        end = ClosestTileCoordinatesV3(end);

        Vector2Int endV2Int = new Vector2Int((int)end.x, (int)end.y);
        Vector2Int startV2Int = new Vector2Int((int)start.x, (int)start.y);

        _mapOccupiedInfo.Deoccupy(startV2Int);
        Occupy(endV2Int);

        float distance = (end - start).magnitude;


        t = 0;
        bool complete = false;

        while (!complete)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(start, end, Mathf.Clamp01(t * moveSpeed / distance));


            if (t * moveSpeed / distance >= 1)
            {
                complete = true;
                transform.position = new Vector3(endV2Int.x, endV2Int.y, 0);
                stepFrom = VectorTools.GetClosestTileCoordinatesV2Int(transform.position);
                stepIndex++;
            }

            yield return null;
        }
    }

    private Path GetPathImmediately(Vector3 start, Vector3 end)
    {
        var newPath = ABPath.Construct(start, end, null);
        AstarPath.StartPath(newPath);
        newPath.BlockUntilCalculated();
        return newPath;
    }



    private Path GetPathAvoidingAllOccupiedTiles(Vector2Int destination)
    {
        var toAvoid = _mapOccupiedInfo.GetAllBlockedTiles();

        foreach (Vector2Int blockedTile in toAvoid)
        {
            if (blockedTile == stepFrom) { continue; }
            BlockNode(blockedTile);
        }

        Path retPath = GetPathImmediately((Vector2)stepFrom, (Vector3Int)destination);

        foreach (Vector2Int blockedTile in toAvoid)
        {
            if (blockedTile == stepFrom) { continue; }
            UnblockNode(blockedTile);
        }

        return retPath;
    }

    private Path GetPathAvoidingTile(Vector2Int tileToAvoid, Vector2Int destination)
    {
        BlockNode(tileToAvoid);
        var ret = GetPathImmediately((Vector2)stepFrom, (Vector3Int)destination);
        UnblockNode(tileToAvoid);
        return ret;
    }


}

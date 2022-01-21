using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEditor;

public class MovementStripped : MonoBehaviour
{
    public float moveSpeed;

    bool initialized = false;
    Coroutine initializer;

    UnitMap unitMap;
    bool stacks = false;

    Path path;
    float t;
    int maxAttempts = 3;
    int attempts = 0;
    float closeEnoughThreshold = 2f;
    int newPaths = 0;
    int maxNewPaths = 3;

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

    bool formationPathing = false;


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
            tSpamMoveToIterations++; // when is this reset? in stopOrder. See if that works.
        }
    }

    private void OnDrawGizmos()
    {
        Handles.Label(transform.position, pathDestination.x + ", " + pathDestination.y);

        Gizmos.DrawLine(new Vector3(pathDestination.x, pathDestination.y, 0), transform.position);
    }

    public void StopOrder()
    {
        followPathCoroutine = false;
        stopOrder = true;
        tSpamMoveToIterations = 0;

    }

    public void MoveTo(Vector3 input, bool formationPathing = false)
    {
        newPaths = 0;
        this.formationPathing = formationPathing;
        shouldMove = true;
        stopOrder = false;
        pathDestination = new Vector2Int((int)ClosestTileCoordinatesV3(input).x, (int)ClosestTileCoordinatesV3(input).y);

        attempts = 0;

        if (!followPathCoroutine)
        {
            followPathCoroutine = true;
            StartCoroutine(FollowPathCoroutine());
        }
    }

    private void OnGotPath(Path p)
    {
        path = p;
    }

    public Vector3 GetDestinationTile()
    {
        return new Vector3(pathDestination.x, pathDestination.y, 0);
    }

    /// <summary>
    /// I don't understand how this works and will probably have to rework it.
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
                newPaths = 0;
            }

            // check if arrived.
            if (stepIndex >= path.vectorPath.Count)
            {
                Debug.Log("arrived1");

                StopOrder();
                yield break;
            }

            if (followingPath)
            {

                // check if next step occupied
                Vector2Int nextStepV2Int = new Vector2Int((int)path.vectorPath[stepIndex].x, (int)path.vectorPath[stepIndex].y);

                if (unitMap.IsUnitAt(nextStepV2Int))
                {
                    attempts++;
                    Debug.Log("Attempts = " + attempts);

                    //if (unitMap.GetUnitAt(nextStepV2Int).GetComponent<MovementStripped>().followPathCoroutine)
                    //{
                    //    yield return new WaitForSeconds(0.1f);
                    //    continue;
                    //}
                    //else
                    //{
                    //    if (Vector2.Distance(transform.position, new Vector2(currentDestination.x, currentDestination.y)) < closeEnoughThreshold)
                    //{
                    //    StopOrder();
                    //}


                    if (attempts > maxAttempts)
                    {
                        if (newPaths > maxNewPaths)
                        {
                            newPaths = 0;
                            StopOrder();
                            yield break;
                        }

                        Path newPath = GetPathAvoidingTile(nextStepV2Int, currentDestination);
                        if (newPath == null)
                        {
                            StopOrder();
                        }
                        OnGotPath(newPath);
                        stepIndex = 1;
                        attempts = 0;

                        newPaths++;
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.25f);
                        }

                        continue;
                    }
                //}

                // check if arrived (again, path may have changed)
                if (stepIndex >= path.vectorPath.Count)
                {
                    Debug.Log("arrived2");

                    StopOrder();
                    yield break;
                }

                if (!stopOrder) // redundant?
                {
                    yield return StartCoroutine(MoveFromToCell(stepFrom, path.vectorPath[stepIndex]));
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
        unitMap = FindObjectOfType<MapState>().unitMap;
        if (unitMap == null)
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
        stepFrom = new Vector2Int((int)closest.x, (int)closest.y);

        Occupy(stepFrom);
    }

    public void Die()
    {
        unitMap.Remove(currentOccupied);
    }

    private void Occupy(Vector2Int tile)
    {
        unitMap.Add(tile, gameObject);
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

        unitMap.Remove(startV2Int);
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

    private void GetPathAndCallback(Vector3 start, Vector3 end)
    {
        var newPath = ABPath.Construct(start, end, OnGotPath);
        AstarPath.StartPath(newPath);
    }



    private Path GetPathAvoidingAllOccupiedTiles(Vector2Int destination)
    {
        var toAvoid = unitMap.GetAllOccupiedTiles();

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

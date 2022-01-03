using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MovableUnit2 : MonoBehaviour
{

    GraphMask graphMask;
    Coroutine movementCoroutine;
    SingleNodeBlocker nodeBlocker;
    Seeker seeker;
    Path path;
    bool moving;
    bool hasStopOrder;
    BlockManager.TraversalProvider traversalProvider;
    private BlockManagerHolder blockManagerHolderRef;


    bool initialized = false;
    Coroutine initializer;

    private MapOccupiedInfo _mapOccupiedInfo;
    private Vector2Int currentBlocked;



    float t;
    int stepIndex = 0;
    Vector2 stepFrom; // current cell until movement to a new cell is completed.
    Vector2 stepTo;
    float moveSpeed = 1;
    Vector2 destinationCache;


    void Update()
    {
        if (!initialized && initializer == null)
        {
            initializer = StartCoroutine(Initialize());
        }
    }

    private IEnumerator Initialize()
    {
        yield return new WaitForEndOfFrame();

        seeker = GetComponent<Seeker>();
        t = 0;
        _mapOccupiedInfo = FindObjectOfType<MapOccupiedInfo>();

        nodeBlocker = GetComponent<SingleNodeBlocker>();

        blockManagerHolderRef = GameObject.Find("BlockManagerObject").GetComponent<BlockManagerHolder>();         // Hardcoded name, will be required in every scene. 

        blockManagerHolderRef.AddBlocker(nodeBlocker);

        traversalProvider = new BlockManager.TraversalProvider(blockManagerHolderRef.blockManager, BlockManager.BlockMode.AllExceptSelector, blockManagerHolderRef.GetObstaclesList());

        
        path = GetPathImmediately(transform.position, ClosestTileCoordinatesV3(transform.position));

        SnapToNearestTile();

        initialized = true;

    }

    private void SnapToNearestTile()
    {
        var closest = ClosestTileCoordinatesV3(transform.position);
        transform.position = closest;
        stepFrom = closest;
    }


    private Vector3 ClosestTileCoordinatesV3(Vector3 position)
    {
        return new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), 0);
    }

    private Path GetPathImmediately(Vector3 start, Vector3 end)
    {
        var path = ABPath.Construct(start, end, null);
        AstarPath.StartPath(path);
        path.BlockUntilCalculated();
        return path;
    }


    private void StartMovementCoroutine()
    {
        stepIndex = 1;
        if (movementCoroutine == null)
        {
            movementCoroutine = StartCoroutine(MovementCoroutine());
        }
    }

    private void OnReceivePath(Path p)
    {
        SetPath(p);
        path.callback = OnReceivePath;
        path.traversalProvider = traversalProvider;
        AstarPath.StartPath(path);
        path.BlockUntilCalculated();
        StartMovementCoroutine();
    }

    private void SetPath(Path p)
    {
        path = p;
    }

    public void MoveTo(Vector2 destination)
    {
        destinationCache = destination;


        if (movementCoroutine != null)
        {
            StopMovingOrder();
        }

        path = GetPathImmediately(stepFrom, destination);
        StartMovementCoroutine();

    }

    public bool MoveTo(Vector2 destination, float speedOverride)
    {
        return true;
    }

    public void StopMovingOrder()
    {
        hasStopOrder = true;
    }

    /// <summary>
    /// Lerps in constant time from start to end. Designed for single-cell movement, but can go any distance.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    private IEnumerator MoveFromToCell(Vector2 start, Vector2 end)
    {
        // quantize values
        start = ClosestTileCoordinatesV3(start);
        end = ClosestTileCoordinatesV3(end);

        float distance = (end - start).magnitude;

        t = 0;
        bool complete = false;
        
        while (!complete)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(start, end, Mathf.Clamp01(t * moveSpeed / distance));
            
            if (t / distance >= 1)
            {
                complete = true;
                SnapToNearestTile();
                stepFrom = end;
            }

            yield return null;
        }
    }
    /// <summary>
    /// Movement Order execution:
    /// 1. Get path
    /// 2. Check next step not occupied
    /// 3. Occupy next step
    /// 4. Move until arrived
    /// 5. Update next step.
    /// </summary>
    /// <returns></returns>
    private IEnumerator MovementCoroutine()
    {
        var iterations = 0;
        while (true)
        {
            if (iterations == 0)
            {
                // first run whatever I want to do
            }
            iterations++;

            
            // check if arrived
            // Possible conditions: t * moveSpeed >= 1, distance < threshold.
            if (t * moveSpeed < 1)
            {
                // has not arrived
                // move transform with lerp
                transform.position = Vector3.Lerp(stepFrom, path.vectorPath[stepIndex], t * moveSpeed);
            }
            else
            {
                // has arrived

                // reset lerp time
                t = 0;

                // set current position
                stepFrom = stepTo;

                // check if done or ordered to stop

                var atPathEnd = path.vectorPath.Count == stepIndex + 1;
                if (atPathEnd || hasStopOrder)
                {
                    // end movement
                    StopCoroutine(movementCoroutine);
                }

                else
                {
                    // advance path
                    stepIndex++;
                    stepTo = path.vectorPath[stepIndex];

                    Path p = ABPath.Construct(transform.position, stepTo); // make a temp path to the next step
                    AstarPath.StartPath(p);                                 // calculate it
                    p.BlockUntilCalculated();                               // now.
                    if (p.error)                                            // if node has become blocked. 
                    {
                        path = GetPathImmediately(stepFrom, destinationCache);                           // Get a new path to the destination.
                    }
                    else
                    {
                        OccupyNode(stepTo);
                    }
                    
                }
            }
            yield return null;
        }

    }

    private void OccupyNode(Vector3 toOccupy)
    {
        Vector2Int toOccupyInt = new Vector2Int((int)toOccupy.x, (int)toOccupy.y);

        _mapOccupiedInfo.Deoccupy(currentBlocked);
        _mapOccupiedInfo.Occupy(toOccupyInt);

        currentBlocked = toOccupyInt;
    }




}

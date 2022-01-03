using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BlockManagerHolder : MonoBehaviour
{


    public BlockManager blockManager;
    private List<SingleNodeBlocker> obstacles;

    public void AddBlocker(SingleNodeBlocker toAdd)
    {
        obstacles.Add(toAdd);
    }

    public void RemoveBlocker(SingleNodeBlocker toRemove)
    {
        if (obstacles.Contains(toRemove))
        {
            obstacles.Remove(toRemove);
        }
        else
        {
            Debug.LogError("Trying to remove blocker that is not in blockmanager");
        }
    }

    public List<SingleNodeBlocker> GetObstaclesList()
    {
        return obstacles;
    }

    void Start()
    {
        blockManager = GetComponent<BlockManager>();
        obstacles = new List<SingleNodeBlocker>();
    }
}

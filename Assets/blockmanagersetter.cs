using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class blockmanagersetter : MonoBehaviour
{
    SingleNodeBlocker singleNodeBlocker;
    BlockManager blockManager;

    private void Awake()
    {
        blockManager = GetComponent<BlockManager>();
        singleNodeBlocker = GetComponent<SingleNodeBlocker>();

        singleNodeBlocker.manager = blockManager;
    }

    private void Update()
    {
        singleNodeBlocker.BlockAtCurrentPosition();
    }
}

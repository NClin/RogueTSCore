using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLineOfSight : MonoBehaviour
{
    [SerializeField]
    private float radius;

    LineOfSight lineOfSight;

    private void Start()
    {
        lineOfSight = FindObjectOfType<LineOfSight>();
    }

    private void Update()
    {
        if (lineOfSight != null)
        {
            lineOfSight.AddVisionForFrame(VectorTools.GetClosestTileCoordinatesV2Int(transform.position), radius);
        }
    }

}

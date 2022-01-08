using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorTools
{
    public static Vector3 GetClosestTileCoordinatesV3(Vector3 position)
    {
        return new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), 0);
    }

    public static Vector3Int GetClosestTileCoordinatesV3Int(Vector3 position)
    {
        return new Vector3Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), 0);
    }


    public static Vector2Int GetClosestTileCoordinatesV2Int(Vector3 position)
    {
        return new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
    }


    public static Vector3 GetBoxExtents(Vector3 start, Vector3 end)
    {
        var boxExtents = (end - start) / 2;
        boxExtents = new Vector3(Mathf.Abs(boxExtents.x), Mathf.Abs(boxExtents.y), Mathf.Abs(boxExtents.z));
        return boxExtents;
    }

    public static Vector3 GetBoxCenter(Vector3 start, Vector3 end)
    {
        var boxCenter = (start + end) / 2;
        Debug.Log(boxCenter);
        return boxCenter;
    }

    public static Vector3[] GetBoxStartEnd(Vector3 center, Vector3 extents)
    {
        // Not yet implemented
        return new Vector3[] { Vector3.zero, Vector3.zero };
    }



}

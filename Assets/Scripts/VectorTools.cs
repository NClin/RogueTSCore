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
        float dx = Mathf.Abs(start.x - end.x);
        float dy = Mathf.Abs(start.y - end.y);
        float dz = Mathf.Abs(start.z - end.z);

        return new Vector3(dx, dy, dz);
    }

    public static Vector3 GetBoxCenter(Vector3 start, Vector3 end)
    {
        var boxCenter = start + ((end-start) / 2);
        return boxCenter;
    }

    public static Vector3[] GetBoxStartEnd(Vector3 center, Vector3 extents)
    {
        // Not yet implemented
        return new Vector3[] { Vector3.zero, Vector3.zero };
    }

    public static List<Vector2Int> BesenhamLine(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> ret = new List<Vector2Int>();

        int w = end.x - start.x;
        int h = end.y - start.y;
        int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
        if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
        if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
        if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
        int longest = Mathf.Abs(w);
        int shortest = Mathf.Abs(h);
        if (!(longest > shortest))
        {
            longest = Mathf.Abs(h);
            shortest = Mathf.Abs(w);
            if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
            dx2 = 0;
        }
        int numerator = longest >> 1;

        Vector2Int index = start;

        for (int i = 0; i <= longest; i++)
        {
            ret.Add(index);
            numerator += shortest;
            if (!(numerator < longest))
            {
                numerator -= longest;
                index.x += dx1;
                index.y += dy1;
            }
            else
            {
                index.x += dx2;
                index.y += dy2;
            }
        }

        return ret;
    }

    public static int GetVectorQuadrant(Vector3 lineStart, Vector3 lineEnd)
    {
        var dx = (lineEnd.x - lineStart.x);
        var dy = (lineEnd.y - lineStart.y);
        int quadrant;
        if (dx > 0)
        {
            if (dy > 0)
            {
                quadrant = 0;
            }
            else
            {
                quadrant = 3;
            }
        }
        else
        {
            if (dy > 0)
            {
                quadrant = 1;
            }
            else
            {
                quadrant = 2;
            }
        }
        return quadrant;
    }
    
    public static Vector2Int GetVectorFormationRowTranslation(Vector3 lineStart, Vector3 lineEnd)
    {
        var dx = (lineEnd.x - lineStart.x);
        var dy = (lineEnd.y - lineStart.y);
        var angle = Mathf.Atan(dy / dx);
        int quadrant;
        if (dx > 0)
        {
            if (dy > 0)
            {
                quadrant = 0;
            }
            else
            {
                quadrant = 3;
            }
        }
        else
        {
            if (dy > 0)
            {
                quadrant = 1;
            }
            else
            {
                quadrant = 2;
            }
        }


        Vector2Int formationRowTranslation = new Vector2Int(0, 0);
        if (quadrant == 0)
        {
            if (angle < 3.14 / 4)
            {
                formationRowTranslation = new Vector2Int(0, -1);
            }
            else
            {
                formationRowTranslation = new Vector2Int(1, -1);
            }
        }
        if (quadrant == 1)
        {
            if (Mathf.Abs(angle) < 3.14 / 4)
            {
                formationRowTranslation = new Vector2Int(1, 1);
            }
            else
            {
                formationRowTranslation = new Vector2Int(1, 0);
            }
        }
        if (quadrant == 2)
        {
            if (Mathf.Abs(angle) < 3.14 / 4)
            {
                formationRowTranslation = new Vector2Int(0, 1);
            }
            else
            {
                formationRowTranslation = new Vector2Int(-1, 1);
            }
        }
        if (quadrant == 3)
        {
            if (Mathf.Abs(angle) < 3.14 / 4)
            {
                formationRowTranslation = new Vector2Int(-1, -1);
            }
            else
            {
                formationRowTranslation = new Vector2Int(-1, 0);
            }
        }

        return formationRowTranslation;
    }

    public static bool ExistsNeighbor(Vector2Int tile, int width, int height, Vector2Int direction)
    {
        if ((tile + direction).x < 0
            || (tile + direction).x >= width
            || (tile + direction).y < 0
            || (tile + direction).y >= height
            )
        {
            return false;
        }
        else
            return true;
    }

}

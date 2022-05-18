using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class UnitMap
{
    private int width, height;
    private GameObject?[,] unitMap;

    public UnitMap(int width, int height)
    {
        this.width = width;
        this.height = height;
        unitMap = new GameObject?[width, height];
        for (int x = 0; x < width - 1; x++)
        {
            for (int y = 0; y < height - 1; y++)
            {
                unitMap[x, y] = null;
            }
        }
    }

    // This is not enforcing exclusivity in case I want units to stack.
    public void Add(Vector2Int tile, GameObject occupier) 
    {
        if (tile.x >= width || tile.y >= height || tile.x < 0 || tile.y < 0) return;
        unitMap[tile.x, tile.y] = occupier;
    }

    public void Remove(Vector2Int tile)
    {
        if (tile.x >= width || tile.y >= height || tile.x < 0 || tile.y < 0) return;
        unitMap[tile.x, tile.y] = null;
    }

    public bool IsUnitAt(Vector2Int tile)
    {
        if (tile.x >= width || tile.y >= height || tile.x < 0 || tile.y < 0) return false;
        return unitMap[tile.x, tile.y] != null;
    }

    public GameObject GetUnitAt(Vector2Int tile)
    {
        if (tile.x >= width || tile.y >= height || tile.x < 0 || tile.y < 0) return null;
        return unitMap[tile.x, tile.y];
    }

    public List<Vector2Int> GetAllOccupiedTiles()
    {
        List<Vector2Int> toReturn = new List<Vector2Int>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (unitMap[x, y] != null)
                {
                    toReturn.Add(new Vector2Int(x, y));
                }
            }
        }
        return toReturn;
    }

    /// <summary>
    /// Untested
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public List<GameObject> GetUnitsInBox(Vector2 a, Vector2 b)
    {
        List<GameObject> units = new List<GameObject>();
        var v2a = VectorTools.GetClosestTileCoordinatesV2Int(a);
        var v2b = VectorTools.GetClosestTileCoordinatesV2Int(b);

        int dx = Mathf.Abs(v2a.x - v2b.x);
        int dy = Mathf.Abs(v2a.y - v2b.y);

        for (int x = v2a.x < v2b.x ? v2a.x : v2b.x; x <= x + dx; x++)
        {
            for (int y = v2a.y < v2b.y ? v2a.y : v2b.y; y <= y + dy; y++)
            {
                var pos = new Vector2Int(x, y);
                if (IsUnitAt(pos))
                {
                    units.Add(GetUnitAt(pos));
                }
            }
        }

        return units;
    }


}

using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FormationStamp : MonoBehaviour
{

    [SerializeField]
    int width;
    [SerializeField]
    int height;
    [SerializeField]
    float rotation;

    List<Vector3Int> stampedTiles;
    Tilemap tilemap;

    public void ClearStamp()
    {
        foreach (Vector3Int tile in stampedTiles)
        {
            tilemap.SetTileFlags(tile, TileFlags.None);
            tilemap.SetColor(tile, Color.white);
        }
        stampedTiles.Clear();
    }

    public void StampLine(Vector3 start, Vector3 end)
    {
        if (tilemap == null)
        {
            tilemap = FindObjectOfType<Tilemap>();
            stampedTiles = new List<Vector3Int>();
        }

        ClearStamp();

        Vector2Int startTile = VectorTools.GetClosestTileCoordinatesV2Int(start);
        Vector2Int endTile = VectorTools.GetClosestTileCoordinatesV2Int(end);

        foreach (Vector3Int tile in besenhamLine(startTile, endTile))
        {
            tilemap.SetTileFlags(tile, TileFlags.None);
            tilemap.SetColor(tile, Color.red);
            stampedTiles.Add(tile);
        }
    }

    void DoStamp()
    {
        var tile = VectorTools.GetClosestTileCoordinatesV3Int(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        tilemap.SetTileFlags(tile, TileFlags.None);
        tilemap.SetColor(tile, Color.red);

        stampedTiles.Add(tile);
    }

    public List<Vector2Int> besenhamLine(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> ret = new List<Vector2Int>();

        int w = end.x - start.x;
        int h = end.y - start.y;
        int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
        if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
        if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
        if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
        int longest = Math.Abs(w);
        int shortest = Math.Abs(h);
        if (!(longest > shortest))
        {
            longest = Math.Abs(h);
            shortest = Math.Abs(w);
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
}

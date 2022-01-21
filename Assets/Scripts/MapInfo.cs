using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MapInfo
{
    private int width, height;
    private TileType[,] tiles;

    public MapInfo(int width, int height)
    {
        this.width = width;
        this.height = height;
        this.tiles = new TileType[width, height];
    }

    public void SetTileType(TileType tileType, Vector2Int pos)
    {
        tiles[pos.x, pos.y] = tileType;
    }

    public TileType GetTileType(Vector2Int pos)
    {
        return tiles[pos.x, pos.y];
    }

    public Vector2Int GetDimensions()
    {
        return new Vector2Int(width, height);
    }

    public int Height()
    {
        return height;
    }

    public int Width()
    {
        return width;
    }
}

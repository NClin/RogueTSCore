using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TileTypeMapDeprecated
{
    public readonly int width, height;
    private TileType[,] tiles;

    public TileTypeMapDeprecated(int width, int height)
    {
        this.width = width;
        this.height = height;
        this.tiles = new TileType[width, height];
    }

    public void SetTileType(TileType tileType, Vector2Int tile)
    {
        if (tile.x >= width || tile.y >= height || tile.x < 0 || tile.y < 0) return;
        else
        {
            tiles[tile.x, tile.y] = tileType;
        }
    }

    public TileType GetTileType(Vector2Int tile)
    {
        if (tile.x >= width || tile.y >= height || tile.x < 0 || tile.y < 0) return TileType.empty;

        return tiles[tile.x, tile.y];
    }
}


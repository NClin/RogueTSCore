using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TileTypeMap
{
    private int width, height;
    private TileType[,] tiles;

    public TileTypeMap(int width, int height)
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

    public TileType[,] GetAllTileTypes()
    {
        return tiles;
    }

    public TileType[] GetNeighborTileTypes(Vector2Int tile)
    {
        List<TileType> outputToArray = new List<TileType>();

        if (VectorTools.ExistsNeighbor(tile, width, height, Vector2Int.up))
        {
            outputToArray.Add(GetTileType(tile + Vector2Int.up));
        }
        if (VectorTools.ExistsNeighbor(tile, width, height, Vector2Int.down))
        {
            outputToArray.Add(GetTileType(tile + Vector2Int.down));
        }
        if (VectorTools.ExistsNeighbor(tile, width, height, Vector2Int.left))
        {
            outputToArray.Add(GetTileType(tile + Vector2Int.left));
        }
        if (VectorTools.ExistsNeighbor(tile, width, height, Vector2Int.right))
        {
            outputToArray.Add(GetTileType(tile + Vector2Int.right));
        }


        return outputToArray.ToArray();

    }

}

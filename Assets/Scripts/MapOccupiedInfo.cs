using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class will know which tiles are occupied for the purpose of pathfinding - Every unit will have a reference to this class, and will announce when it begins to move to (occupies) a square, and when it begins to move away from the square.
/// </summary>
public class MapOccupiedInfo : MonoBehaviour
{
    private int width, height;
    private bool[,] occupied;

    public void Awake()
    {
        // placeholders until map generation can set these.
        width = 25;
        height = 25;

        occupied = new bool[height, width];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                occupied[x, y] = false;
            }
        }
    }
    public void Occupy(Vector2Int tile)
    {
        occupied[tile.x, tile.y] = true;
    }

    public void Deoccupy(Vector2Int tile)
    {
        occupied[tile.x, tile.y] = false;
    }

    public bool IsOccupied(Vector2Int tile)
    {
        return occupied[tile.x, tile.y];
    }

    public List<Vector2Int> GetAllBlockedTiles()
    {
        List<Vector2Int> toReturn = new List<Vector2Int>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (occupied[x, y] == true)
                {
                    toReturn.Add(new Vector2Int(x, y));
                }
            }
        }
        return toReturn;
    }

}

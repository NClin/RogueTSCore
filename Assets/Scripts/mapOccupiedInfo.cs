using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapOccupiedInfo : MonoBehaviour
{
    private int width, height;
    private GameObject?[,] occupationMap;

    public void Awake()
    {
        EnforceSingleton();

        // placeholders until map generation can set these.
        width = 60;
        height = 60;

        occupationMap = new GameObject[height, width];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                occupationMap[x, y] = null;
            }
        }
    }
    public void Occupy(Vector2Int tile, GameObject occupier)
    {
        if (tile.x >= width || tile.y >= height || tile.x < 0 || tile.y < 0) return;

        occupationMap[tile.x, tile.y] = occupier;
    }

    public void Deoccupy(Vector2Int tile)
    {
        if (tile.x >= width || tile.y >= height || tile.x < 0 || tile.y < 0) return;

        occupationMap[tile.x, tile.y] = null;
    }

    public bool IsOccupied(Vector2Int tile)
    {
        if (tile.x >= width || tile.y >= height || tile.x < 0 || tile.y < 0) return false;

        return occupationMap[tile.x, tile.y] != null;
    }

    public List<Vector2Int> GetAllBlockedTiles()
    {
        List<Vector2Int> toReturn = new List<Vector2Int>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (occupationMap[x, y] == true)
                {
                    toReturn.Add(new Vector2Int(x, y));
                }
            }
        }
        return toReturn;
    }

    private void EnforceSingleton()
    {
        var check = FindObjectOfType<mapOccupiedInfo>();
        if (check != this)
        {
            Debug.LogError("MapOccupiedInfo already exists");
            Destroy(this);
        }
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesMap
{
    int width, height;
    private ResourceNode?[,] resourcesMap;

    public ResourcesMap(int width, int height)
    {
        resourcesMap = new ResourceNode?[width, height];
        this.width = width;
        this.height = height;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                resourcesMap[x, y] = null;
            }
        }
    }

    public ResourceNode? GetNodeAt(Vector2Int tile)
    {
        if (tile.x >= width || tile.y >= height || tile.x < 0 || tile.y < 0) return null;
        return resourcesMap[tile.x, tile.y];
    }

    public bool IsNodeAt(Vector2Int tile)
    {
        return resourcesMap[tile.x, tile.y] != null;
    }

    public bool AddNode(Vector2Int tile, ResourceNode toAdd)
    {
        if (tile.x >= width || tile.y >= height || tile.x < 0 || tile.y < 0) return false;

        if (IsNodeAt(tile)) return false;

        resourcesMap[tile.x, tile.y] = toAdd;
        return true;
    }

    public List<ResourceNode> GetNearbyResourceNodes(Vector2 point, float range)
    {
        var nodes = new List<ResourceNode>();

        foreach (var loc in resourcesMap)
        {
            if (loc != null && Vector2.Distance(loc.position, point) < range)
            {
                nodes.Add(loc);
            }
        }

        return nodes;
    }

}

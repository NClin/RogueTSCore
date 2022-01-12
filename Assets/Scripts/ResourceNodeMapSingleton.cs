using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Map))]

public class ResourceNodeMapSingleton : MonoBehaviour
{
    [SerializeField]
    private GameObject nodePrefab;

    private ResourceNode?[,] resourceNodeMap;

    private Map map;

    public void Awake()
    {
        EnforceSingleton();

        var check = FindObjectOfType<ResourceNodeMapSingleton>();
        if (check != this)
        {
            Debug.LogError("ResourceNodeMapSingleton already exists");
            Destroy(this);
        }
        CheckIntegrity();



    }

    public void Generate(int height, int width)
    {
        resourceNodeMap = new ResourceNode?[height, width];
        Debug.Log("initializing resourceNodeMap");
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
            }
        }
    }

    public ResourceNode? GetNodeAt(Vector2Int tile)
    {
        CheckIntegrity();
        return resourceNodeMap[tile.x, tile.y];
    }

    private void EnforceSingleton()
    {
        var check = FindObjectOfType<ResourceNodeMapSingleton>();
        if (check != this)
        {
            Debug.LogError("resourceNodeMap already exists");
            Destroy(this);
        }
    }

    private void CheckIntegrity()
    {
        if (map == null)
        {
            map = FindObjectOfType<Map>();
            map.resourceNodeMapSingleton = this;
        }
        if (map == null)
        {
            Debug.LogError("Map not found by ResourceNodeMapSingleton. Should be tightly coupled.");
        }
        if (resourceNodeMap == null)
        {
            Generate(map.height, map.width);
        }
    }

    public bool IsNodeAt(Vector2Int tile)
    {
        CheckIntegrity(); // remove these later if they never trigger? The one in Awake() should be sufficient.

        if (resourceNodeMap[tile.x, tile.y] != null)
        { return true; }
        else
        { return false; }    
    }

    public bool AddNode(Vector2Int tile, int nodeStock, ResourceType resourceType)
    {
        CheckIntegrity(); // remove these later if they never trigger? The one in Awake() should be sufficient.

        if (IsNodeAt(tile))
         {
             return false;
         }

        var toSpawn = Instantiate(nodePrefab, new Vector3(tile.x, tile.y, 0), Quaternion.identity);
        toSpawn.GetComponent<ResourceNode>().SetStock(nodeStock, resourceType);
        resourceNodeMap[tile.x, tile.y] = toSpawn.GetComponent<ResourceNode>();
        return true;
    }

}

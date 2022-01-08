using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//[RequireComponent(typeof(Map))] 
public class MapGenerator : MonoBehaviour
{
    Tilemap tilemap;
    //Map map;

    [SerializeField]
    Tile wallTile;
    [SerializeField]
    Tile groundTile;

    bool scanned = false;
    bool mapGenerated = false;

    float offsetX = 0.5f;
    float offsetY = 0.5f;

    [SerializeField]
    int width = 60;
    [SerializeField]
    int height = 60;

    // Start is called before the first frame update
    void Awake()
    {
        //map = GetComponent<Map>();
        tilemap = GetComponentInChildren<Tilemap>();
        //width = map.width;
        //height = map.height;
    }


    private void Update()
    {
        if (!mapGenerated)
        {
            RectangleMap(width, height);
            return;
        }

        if (!scanned)
        {
            AstarPath.active.data.gridGraph.SetDimensions(width, height, 1f);
            var center = new Vector3(width / 2 - offsetX, height / 2 - offsetY, 0);
            AstarPath.active.data.gridGraph.center = center;
            AstarPath.active.Scan();
            scanned = true;
        }
    }

    void RectangleMap(int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var pos = new Vector3Int(x, y, 0);
                tilemap.SetTile(pos, groundTile);

                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    tilemap.SetTile(pos, wallTile);
                }
            }
        }

        tilemap.RefreshAllTiles();

        mapGenerated = true;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// To be attached to parent map component and fed mapInfo.
/// </summary>
public class TileMapTileHandler : MonoBehaviour
{
    Tilemap tileSpriteMap;
    [SerializeField]
    Tile baseTile;

    private PathfindingHandler pathfindingHandler;

    private Dictionary<TileType, Color> tileTypeColors = new Dictionary<TileType, Color>();
    private Dictionary<TileType, int> tileTypeLayers = new Dictionary<TileType, int>();

    void Awake()
    {
        tileSpriteMap = GetComponent<Tilemap>();
        {
            if (tileSpriteMap == null)
            {
                Debug.LogError("tilemap not found");
            }
        }
    }

    private void Start()
    {
        pathfindingHandler = FindObjectOfType<PathfindingHandler>();
        if (pathfindingHandler == null)
        {
            Debug.LogError("pathfindingHandler not found");
        }


        // tile color definitions
        tileTypeColors.Add(TileType.empty, Color.yellow / 1.3f);
        tileTypeColors.Add(TileType.wall, Color.blue / 1.3f);


        // tile layer definitions
        tileTypeLayers.Add(TileType.empty, 6);
        tileTypeLayers.Add(TileType.wall, 7);
    }

    public void InstantiateMap(TileTypeMap mapToGenerate)
    {
        Debug.Log("Instantiating map");
        int width = mapToGenerate.GetDimensions().x;
        int height = mapToGenerate.GetDimensions().y;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var pos = new Vector3Int(x, y, 0);
                tileSpriteMap.SetTile(pos, baseTile);

                var placed = tileSpriteMap.GetTile<Tile>(pos);
                var tileType = mapToGenerate.GetTileType(new Vector2Int(pos.x, pos.y));


                if (tileType == TileType.wall)
                {
                    placed.colliderType = Tile.ColliderType.Grid;
                }
                if (tileType == TileType.empty)
                {
                    placed.colliderType = Tile.ColliderType.None;
                }

                Color col;
                tileTypeColors.TryGetValue(tileType, out col);
                if (col != null)
                {
                    placed.color = col;
                }
            }
        }

        tileSpriteMap.RefreshAllTiles();
        Debug.Log("done");

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// To be attached to parent map component and fed mapInfo.
/// </summary>
public class MapTilePlacer : MonoBehaviour
{
    Tilemap tilemap;
    [SerializeField]
    Tile baseTile;
    MapTile mapTile;

    PathfindingGridScanner pathfindingGridScanner;

    void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();

        pathfindingGridScanner = new PathfindingGridScanner();
    }

    public void InstantiateMap(MapInfo mapToGenerate)
    {
        int width = mapToGenerate.GetDimensions().x;
        int height = mapToGenerate.GetDimensions().y;
        int wallCount = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var pos = new Vector3Int(x, y, 0);
                mapTile = new MapTile();
                mapTile.sprite = baseTile.sprite;
                tilemap.SetTile(pos, mapTile);

                var placed = tilemap.GetTile<MapTile>(pos);
                if (mapToGenerate.GetTileType(new Vector2Int(pos.x, pos.y)) == TileType.wall)
                {
                    wallCount++;
                    placed.colliderType = Tile.ColliderType.Grid;
                    placed.color = Color.black;
                    Debug.Log(placed.debug);
                }
                if (mapToGenerate.GetTileType(new Vector2Int(pos.x, pos.y)) == TileType.empty)
                {
                    placed.colliderType = Tile.ColliderType.None;
                }
            }
        }

        tilemap.RefreshAllTiles();
        Debug.Log("done");

        StartCoroutine(pathfindingGridScanner.ScanAtEndOfFrame(height, width));
        
    }
}

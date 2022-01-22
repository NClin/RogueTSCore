using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTileHandler : MonoBehaviour
{
    /// <summary>
    /// To be attached to parent map component and called by anything which wants to change a map tile.
    /// </summary>
    public GameObject baseTile;
    private GameObject[,] mapTiles;
    private PathfindingHandler pathfindingHandler;

    private Dictionary<TileType, Color> tileTypeColors = new Dictionary<TileType, Color>();
    private Dictionary<TileType, int> tileTypeLayers = new Dictionary<TileType, int>();



    private void Start()
    {
        pathfindingHandler = FindObjectOfType<PathfindingHandler>();
        if (pathfindingHandler == null)
        {
            Debug.LogError("pathfindingHandler not found");
        }


        // tile color definitions
        tileTypeColors.Add(TileType.empty, Color.yellow / 2);
        tileTypeColors.Add(TileType.wall, Color.blue / 2);


        // tile layer definitions
        tileTypeLayers.Add(TileType.empty, 6);
        tileTypeLayers.Add(TileType.wall, 7);
    }

    public void InstantiateMap(TileTypeMap mapToGenerate)
    {
        int width = mapToGenerate.GetDimensions().x;
        int height = mapToGenerate.GetDimensions().y;
        mapTiles = new GameObject[width, height];

        pathfindingHandler.GenerateGraph(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var pos = new Vector2Int(x, y);

                var toPlace = Instantiate(baseTile, transform);
                mapTiles[pos.x, pos.y] = toPlace;
                toPlace.transform.position = new Vector3(pos.x, pos.y, 0);

                if (mapToGenerate.GetTileType(pos) == TileType.wall)
                {
                    SetTileType(pos, TileType.wall);
                }
                if (mapToGenerate.GetTileType(new Vector2Int(pos.x, pos.y)) == TileType.empty)
                {
                    SetTileType(pos, TileType.empty);
                }
            }
        }

        pathfindingHandler.ScanGraphCorners(width, height);
    }

    public void SetTileColor(Vector2Int tile, Color color)
    {
        var propID = Shader.PropertyToID("_Color");
        var meshRenderer = mapTiles[tile.x, tile.y].GetComponent<MeshRenderer>();

        MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
        meshRenderer.GetPropertyBlock(_propBlock);
        _propBlock.SetColor(propID, color);
        meshRenderer.SetPropertyBlock(_propBlock);
    }

    public void SetTileType(Vector2Int tile, TileType tileType)
    {

        mapTiles[tile.x, tile.y].GetComponent<MapTileContainer>().mapTile.tiletype = tileType;
        pathfindingHandler.UpdateTileToType(tile, tileType);

        if (tileType == TileType.wall)
        {
            SetTileCollisionLayer(tile, 7);
        }
        else
        {
            SetTileCollisionLayer(tile, 6);
        }

        Color col;
        tileTypeColors.TryGetValue(tileType, out col);
        SetTileColor(tile, col);
    }

    private void SetTileCollisionLayer(Vector2Int tile, int layerID)
    {
        mapTiles[tile.x, tile.y].layer = layerID;
    }

    
}


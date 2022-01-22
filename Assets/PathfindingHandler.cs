using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

/// <summary>
/// Should be Singleton? Add enforcement?
/// </summary>
public class PathfindingHandler : MonoBehaviour
{
    AstarData astarData;
    GridGraph gridGraph;

    public void GenerateAndScanGraph(int width, int height)
    {
        GenerateGraph(width, height);
        ScanGraphCorners(width, height);
    }

    public void GenerateGraph(int width, int height)
    {
        astarData = AstarPath.active.data;
        gridGraph = astarData.AddGraph(typeof(GridGraph)) as GridGraph;
        gridGraph.SetDimensions(width, height, 1);
        gridGraph.is2D = true;

        float xOffset = width % 2 == 0 ? -0.5f : 0;
        float yOffset = height % 2 == 0 ? -0.5f : 0;
        gridGraph.center = transform.position + new Vector3(width / 2 + xOffset, height / 2 + yOffset, 0);

        gridGraph.Scan();

        /// Connections:
        ///         Y
        ///         |
        ///         |
        ///
        ///      6  2  5
        ///       \ | /
        /// --  3 - X - 1  ----- X
        ///       / | \
        ///      7  0  4
        ///
        ///         |
        ///         |
        /// 



        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridNode toSet = (GridNode)gridGraph.GetNode(x, y);

                //set all true
                gridGraph.SetNodeConnection(toSet, 0, true);
                gridGraph.SetNodeConnection(toSet, 7, true);
                gridGraph.SetNodeConnection(toSet, 4, true);
                gridGraph.SetNodeConnection(toSet, 6, true);
                gridGraph.SetNodeConnection(toSet, 2, true);
                gridGraph.SetNodeConnection(toSet, 5, true);
                gridGraph.SetNodeConnection(toSet, 1, true);
                gridGraph.SetNodeConnection(toSet, 3, true);

                // remove border connections
                if (y == 0)
                {
                    gridGraph.SetNodeConnection(toSet, 0, false);
                    gridGraph.SetNodeConnection(toSet, 7, false);
                    gridGraph.SetNodeConnection(toSet, 4, false);
                }
                if (y == height - 1)
                {
                    gridGraph.SetNodeConnection(toSet, 6, false);
                    gridGraph.SetNodeConnection(toSet, 2, false);
                    gridGraph.SetNodeConnection(toSet, 5, false);
                }
                if (x == 0)
                {
                    gridGraph.SetNodeConnection(toSet, 3, false);
                    gridGraph.SetNodeConnection(toSet, 7, false);
                    gridGraph.SetNodeConnection(toSet, 6, false);

                }
                if (x == width - 1)
                {
                    gridGraph.SetNodeConnection(toSet, 5, false);
                    gridGraph.SetNodeConnection(toSet, 1, false);
                    gridGraph.SetNodeConnection(toSet, 4, false);
                }


            }
        }
    }

     public void ScanGraphCorners(int width, int height)
    {

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridNode toSet = (GridNode)gridGraph.GetNode(x, y);
                //remove corner connections
                if (x != 0 && gridGraph.GetNode(x - 1, y).Walkable == false)
                {
                    gridGraph.SetNodeConnection(toSet, 6, false);
                    gridGraph.SetNodeConnection(toSet, 7, false);
                }
                if (x != width - 1 && gridGraph.GetNode(x + 1, y).Walkable == false)
                {
                    gridGraph.SetNodeConnection(toSet, 5, false);
                    gridGraph.SetNodeConnection(toSet, 4, false);
                }
                if (y != 0 && gridGraph.GetNode(x, y - 1).Walkable == false)
                {
                    gridGraph.SetNodeConnection(toSet, 7, false);
                    gridGraph.SetNodeConnection(toSet, 4, false);
                }
                if (y != height - 1 && gridGraph.GetNode(x, y + 1).Walkable == false)
                {
                    gridGraph.SetNodeConnection(toSet, 6, false);
                    gridGraph.SetNodeConnection(toSet, 5, false);
                }
            }
        }
    }

    public void SetWalkable(Vector2Int tile)
    {
        gridGraph.nodes[tile.x+gridGraph.width*tile.y].Walkable = true;
    }

    public void SetBlocked(Vector2Int tile)
    {
        gridGraph.nodes[tile.x + gridGraph.width * tile.y].Walkable = false;
    }

    public bool IsWalkable(Vector2Int tile)
    {
        return gridGraph.nodes[tile.x + gridGraph.width * tile.y].Walkable;
    }

    /// <summary>
    /// Designed to be called by MapTileHandler when tiles are updated.
    /// </summary>
    /// <param name="tile"></param>
    public void UpdateTileToType(Vector2Int tile, TileType tileType)
    {
        if (tileType == TileType.wall)
        {
            SetBlocked(tile);
        }
        if (tileType == TileType.empty)
        {
            SetWalkable(tile);
        }
    }

    public void UpdateAllTiles(TileTypeMap tileTypeMap)
    {
        var dimensions = tileTypeMap.GetDimensions();

        if (gridGraph == null)
        {
            GenerateGraph(dimensions.x, dimensions.y);
        }
        
        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                var currentTile = new Vector2Int(x, y);
                UpdateTileToType(currentTile, tileTypeMap.GetTileType(currentTile));
            }
        }

        ScanGraphCorners(dimensions.x, dimensions.y);
    }
}

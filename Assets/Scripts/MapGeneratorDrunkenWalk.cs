using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorDrunkenWalk
{
    public TileTypeMap RandomWalk(int width, int height, float approxCoveragePercent, int maxWalk = 25, int walkWidth = 1)
    {
        return DoRandomWalk(width, height, approxCoveragePercent, maxWalk, walkWidth);
    }


    private TileTypeMap DoRandomWalk(int width, int height, float approxCoveragePercent, int maxWalk = 40, int walkWidth = 1)
    {
        TileTypeMap mapInfo = new TileTypeMap(width, height);

        if (approxCoveragePercent > 100)
        {
            Debug.LogError("Drunken walk map generator cannot clear more than 100% of tiles");
        }

        // fill with walls
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                mapInfo.SetTileType(TileType.wall, new Vector2Int(x, y));
            }
        }

        int tileCount = width * height;
        int totalStepsTaken = 0;
        Vector2Int currentTile = new Vector2Int(Random.Range(0, width - 1), Random.Range(0, height - 1));
        int currentWalkLength = 0;


        // walk through walls
        while (totalStepsTaken / tileCount < approxCoveragePercent / 100)
        {
            if (currentWalkLength > maxWalk)
            {

                currentTile = new Vector2Int(Random.Range(0, width - 1), Random.Range(0, height - 1));
                mapInfo.SetTileType(TileType.empty, currentTile);

                currentWalkLength = 0;
            }

            int roll = Random.Range(0, 4); // NESW
            Vector2Int step;

            //N
            if (roll == 0 && currentTile.y < height - 1)
            {
                step = new Vector2Int(currentTile.x, currentTile.y + 1);
                mapInfo.SetTileType(TileType.empty, step);
                totalStepsTaken++;
                currentWalkLength++;

                currentTile = step;
                continue;
            }

            //E
            if (roll == 1 && currentTile.x < width - 1)
            {
                step = new Vector2Int(currentTile.x + 1, currentTile.y);
                mapInfo.SetTileType(TileType.empty, step);
                totalStepsTaken++;
                currentTile = step;
                currentWalkLength++;

                continue;
            }

            //S
            if (roll == 2 && currentTile.y > 0)
            {
                step = new Vector2Int(currentTile.x, currentTile.y - 1);
                mapInfo.SetTileType(TileType.empty, step);
                totalStepsTaken++;
                currentWalkLength++;
                currentTile = step;

                continue;
            }

            //W
            if (roll == 3 && currentTile.x > 0)
            {
                step = new Vector2Int(currentTile.x - 1, currentTile.y);
                mapInfo.SetTileType(TileType.empty, step);
                totalStepsTaken++;
                currentWalkLength++;
                currentTile = step;

                continue;
            }

        }

        // remove isolated wall tiles

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int tile = new Vector2Int(x, y);

                if (mapInfo.GetTileType(tile) == TileType.wall)
                {
                    var neighborTileTypes = mapInfo.GetNeighborTileTypes(tile);
                }
                
            }
        }
        return mapInfo;
    }


    
    }


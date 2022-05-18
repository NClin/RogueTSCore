using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerationHandler : MonoBehaviour
{

    MapGeneratorDrunkenWalk drunkenWalkMapInfoGenerator;
    MapState mapState;

    [SerializeField]
    Vector2Int dimensions;
    [SerializeField]
    int walkWidth;
    [SerializeField]
    float coverage;
    [SerializeField]
    int massNodes;
    [SerializeField]
    int massNodeValue;
    [SerializeField]
    int dataSpawns;
    [SerializeField]
    ResourceNodeHandler resourceNodeHandler;
    [SerializeField]
    private GameObject dataObject;
    [SerializeField]
    private GameObject exitObject;

    private void Start()
    {
        // create map
        drunkenWalkMapInfoGenerator = new MapGeneratorDrunkenWalk();
        mapState = FindObjectOfType<MapState>();
        Debug.Log("generating map");
        TileTypeMap generatedMap = drunkenWalkMapInfoGenerator.RandomWalk(dimensions.x, dimensions.y, coverage, walkWidth);

        mapState.SetTileTypeMap(generatedMap);
        FindObjectOfType<LineOfSight>().SetScale(dimensions.x, dimensions.y);

        // spawn resources
        for (int i = 0; i <= massNodes; i++)
        {
            Vector2Int placement = new Vector2Int(Random.Range(1, dimensions.x - 1), Random.Range(1, dimensions.y - 1));
            if (mapState.tileTypeMap.GetTileType(placement) == TileType.empty
                && !mapState.resourcesMap.IsNodeAt(placement))
            {
                resourceNodeHandler.AddResourceNode(placement, massNodeValue);
            }
        }

        for (int i = 0; i <= dataSpawns; i++)
        {
            Vector2Int placement = new Vector2Int(Random.Range(1, dimensions.x - 1), Random.Range(1, dimensions.y - 1));
            if (mapState.tileTypeMap.GetTileType(placement) == TileType.empty
                && !mapState.resourcesMap.IsNodeAt(placement))
            {
                var placementV3 = new Vector3(placement.x, placement.y, 0);
                Instantiate(dataObject, placementV3, Quaternion.identity);
            }
        }

        // TODO: check path from caravan to portal exists.

        // spawn exit
        bool exitSpawned = false;
        while (!exitSpawned)
        {
            Vector2Int placement = new Vector2Int(Random.Range(1, dimensions.x - 1), Random.Range(1, dimensions.y - 1));

            if (mapState.tileTypeMap.GetTileType(placement) == TileType.empty
                && !mapState.resourcesMap.IsNodeAt(placement))
            {
                var placementV3 = new Vector3(placement.x, placement.y, 0);
                Instantiate(exitObject, placementV3, Quaternion.identity);
                exitSpawned = true;
            }
        }
    }
}

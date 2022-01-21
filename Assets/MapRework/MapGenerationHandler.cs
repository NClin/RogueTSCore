using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerationHandler : MonoBehaviour
{

    MapGeneratorDrunkenWalk drunkenWalkMapInfoGenerator;
    MapTileHandler mapTileHandler;
    MapState mapState;

    [SerializeField]
    Vector2Int dimensions;

    private void Start()
    {
        mapTileHandler = FindObjectOfType<MapTileHandler>();
        drunkenWalkMapInfoGenerator = new MapGeneratorDrunkenWalk();
        mapState = FindObjectOfType<MapState>();

        MapInfo generatedMap = drunkenWalkMapInfoGenerator.RandomWalk(dimensions.x, dimensions.y, 25);

        mapTileHandler.InstantiateMap(generatedMap);
        mapState.SetTileTypeMap(generatedMap);
    }
}

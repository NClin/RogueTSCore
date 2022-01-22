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

    private void Start()
    {
        drunkenWalkMapInfoGenerator = new MapGeneratorDrunkenWalk();
        mapState = FindObjectOfType<MapState>();
        Debug.Log("generating map");
        TileTypeMap generatedMap = drunkenWalkMapInfoGenerator.RandomWalk(dimensions.x, dimensions.y, coverage, walkWidth);

        mapState.SetTileTypeMap(generatedMap);
        FindObjectOfType<LineOfSight>().SetScale(dimensions.x, dimensions.y);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHostiles : MonoBehaviour
{
    [SerializeField]
    private int enemiesSpawnedPerTick;
    [SerializeField]
    private float spawnFrequency;
    [SerializeField]
    private GameObject enemyBase;

    private float timeSinceSpawn = float.MaxValue;

    private MapState mapState;
    Vector2Int dimensions;

    void Start()
    {
        mapState = GameObject.FindObjectOfType<MapState>();
        dimensions = mapState.tileTypeMap.GetDimensions();
    }

    // Update is called once per frame
    void Update()
    {
        if (dimensions.x == 0) dimensions = mapState.tileTypeMap.GetDimensions();

        timeSinceSpawn += Time.deltaTime;

        if (timeSinceSpawn < spawnFrequency) return;
        else
        {
            SpawnEnemies();
            timeSinceSpawn = 0;
        }


    }

    void SpawnEnemies()
    {
        for (int i = 0; i <= enemiesSpawnedPerTick; i++)
        {
            Vector2Int placement = new Vector2Int(Random.Range(1, dimensions.x - 1), Random.Range(1, dimensions.y - 1));
            if (mapState.tileTypeMap.GetTileType(placement) == TileType.empty
                && !mapState.resourcesMap.IsNodeAt(placement))
            {
                var placementV3 = new Vector3(placement.x, placement.y, 0);
                Instantiate(enemyBase, placementV3, Quaternion.identity);
            }
        }
    }
}

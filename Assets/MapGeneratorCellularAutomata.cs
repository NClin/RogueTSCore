using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Broken by MapInfo Changes, easy fix (dimensions input)

//[RequireComponent(typeof(Map))] 

public class MapGeneratorCellularAutomata : MonoBehaviour
{
    Tilemap tilemap;
    TileTypeMap map;

    [SerializeField]
    Tile wallTile;
    [SerializeField]
    Tile groundTile;
    

    bool scanned = false;
    bool mapGenerated = false;

    float offsetX = 0.5f;
    float offsetY = 0.5f;


    float t = 0;
    [SerializeField]
    float changeFrequency = 1;
    [SerializeField]
    float changePhase = 0f; // variation would be a little more effort to put in.
    [SerializeField]
    int tilesPerIteration = 50;
    [SerializeField]
    bool setAll = false;

    [SerializeField]
    int maxIterations;

    [SerializeField]
    int iterationsReadOnly;


    // Start is called before the first frame update
    void Awake()
    {
        //map = GetComponent<MapInfo>();
        tilemap = GetComponentInChildren<Tilemap>();
    }

    //private void Start()
    //{
    //    StartCoroutine(RandomSeed(map.width, map.height));
    //}

    //private void Update()
    //{
    //    t += Time.deltaTime;

    //    if (t > changeFrequency + Random.Range(-changePhase, changePhase))
    //    {
    //        t = 0;

    //        DoCellUpdate();

    //    }

        //if (!scanned)
        //{
        //    AstarPath.active.data.gridGraph.SetDimensions(width, height, 1f);
        //    var center = new Vector3(width / 2 - offsetX, height / 2 - offsetY, 0);
        //    AstarPath.active.data.gridGraph.center = center;
        //    AstarPath.active.Scan();
        //    scanned = true;
        //}
    //}

    //private void SetEachTileToRandomNeighbor()
    //{
    //    for (int x = 0; x < map.width; x++)
    //    {
    //        for (int y = 0; y < map.height; y++)
    //        {

    //            List<Tile> neighbors = new List<Tile>();

    //            Tile test;
    //            test = (Tile)tilemap.GetTile(new Vector3Int(x, y + 1, 0));
    //            if (test != null)
    //            {
    //                neighbors.Add(test);
    //            }

    //            test = (Tile)tilemap.GetTile(new Vector3Int(x, y + -1, 0));
    //            if (test != null)
    //            {
    //                neighbors.Add(test);
    //            }

    //            test = (Tile)tilemap.GetTile(new Vector3Int(x + 1, y, 0));
    //            if (test != null)
    //            {
    //                neighbors.Add(test);
    //            }

    //            test = (Tile)tilemap.GetTile(new Vector3Int(x + -1, y, 0));
    //            if (test != null)
    //            {
    //                neighbors.Add(test);
    //            }

    //            tilemap.SetTile(new Vector3Int(x, y, 0), neighbors[Random.Range(0, neighbors.Count)]);

    //        }
    //    }
    //}

    //private void SetRandomTilesToRandomNeighbor(int numberOfTiles)
    //{

    //    int i = 0;
    //    while (i < numberOfTiles)
    //    {
    //        i++;
    //        int x = Random.Range(0, map.width);
    //        int y = Random.Range(0, map.height);

    //        List<Tile> neighbors = new List<Tile>();

    //        Tile test;
    //        test = (Tile)tilemap.GetTile(new Vector3Int(x, y + 1, 0));
    //        if (test != null)
    //        {
    //            neighbors.Add(test);
    //        }

    //        test = (Tile)tilemap.GetTile(new Vector3Int(x, y + -1, 0));
    //        if (test != null)
    //        {
    //            neighbors.Add(test);
    //        }

    //        test = (Tile)tilemap.GetTile(new Vector3Int(x + 1, y, 0));
    //        if (test != null)
    //        {
    //            neighbors.Add(test);
    //        }

    //        test = (Tile)tilemap.GetTile(new Vector3Int(x + -1, y, 0));
    //        if (test != null)
    //        {
    //            neighbors.Add(test);
    //        }

    //        tilemap.SetTile(new Vector3Int(x, y, 0), neighbors[Random.Range(0, neighbors.Count)]);
            
    //    }
        
    //}

    //private void DoCellUpdate()
    //{
    //    if (iterationsReadOnly < maxIterations)

    //    {
    //        if (setAll)
    //        {
    //            SetEachTileToRandomNeighbor();
    //        }
    //        else
    //        {
    //            SetRandomTilesToRandomNeighbor(tilesPerIteration);
    //        }

    //        iterationsReadOnly++;
    //    }
    //}

    bool RollAOutOfB(int a, int b)
    {
        if (a < 0 || b <= 0)
        {
            Debug.LogError("Invalid roll fails automatically");
            return false;
        }
        int roll = Random.Range((int)0, (int)b);
        return roll >= a;
    }

    IEnumerator RandomSeed(int width, int height)
    {
        yield return new WaitForEndOfFrame();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                
                var pos = new Vector3Int(x, y, 0);
                Tile toSet = RollAOutOfB(1,2) ? wallTile : groundTile;
                tilemap.SetTile(pos, toSet);

                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    tilemap.SetTile(pos, wallTile);
                }
            }
        }

        tilemap.RefreshAllTiles();

    }

}

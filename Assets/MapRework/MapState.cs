using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The MonoBehaviour which tracks unit and resource positions. See TileTypeMap, UnitMap, or ResourcesMap for their individual functions.
/// </summary>
public class MapState : MonoBehaviour
{
    [HideInInspector]
    public TileTypeMap tileTypeMap;
    [HideInInspector]
    public UnitMap unitMap;
    [HideInInspector]
    public ResourcesMap resourcesMap;


    private LineOfSight lineOfSight;
    private TileMapIndexTexture tileTypeIndexTexture;
    private PathfindingHandler pathfindingHandler;

    /// <summary>
    /// Sets entire map to given TileTypeMap. To be called by generation scripts.
    /// </summary>
    /// <param name="mapToSet"></param>
    public void SetTileTypeMap(TileTypeMap mapToSet)
    {
        tileTypeMap = mapToSet;
        unitMap = new UnitMap(mapToSet.Height(), mapToSet.Width());
        resourcesMap = new ResourcesMap(mapToSet.Height(), mapToSet.Width());
        lineOfSight = FindObjectOfType<LineOfSight>();
        lineOfSight.InitializeLoS(mapToSet.Height(), mapToSet.Width());
        tileTypeIndexTexture.GenerateFromTileTypeMap(mapToSet);
        pathfindingHandler.UpdateAllTiles(mapToSet);
    }


    private void Start()
    {
        tileTypeIndexTexture = FindObjectOfType<TileMapIndexTexture>();
        pathfindingHandler = FindObjectOfType<PathfindingHandler>();
    }

}

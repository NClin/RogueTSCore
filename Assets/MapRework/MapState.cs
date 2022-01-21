using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The MonoBehaviour which tracks unit and resource positions. See TileTypeMap, UnitMap, or ResourcesMap for their individual functions.
/// </summary>
public class MapState : MonoBehaviour
{
    [HideInInspector]
    public MapInfo tileTypeMap;
    [HideInInspector]
    public UnitMap unitMap;
    [HideInInspector]
    public ResourcesMap resourcesMap;

    private MapTileHandler mapTileHandler;

    /// <summary>
    /// Sets entire map to given TileTypeMap. To be called by generation scripts.
    /// </summary>
    /// <param name="mapToSet"></param>
    public void SetTileTypeMap(MapInfo mapToSet)
    {
        tileTypeMap = mapToSet;
        unitMap = new UnitMap(tileTypeMap.Height(), tileTypeMap.Width());
        resourcesMap = new ResourcesMap(tileTypeMap.Height(), tileTypeMap.Width());
    }

    private void Start()
    {
        mapTileHandler = FindObjectOfType<MapTileHandler>();
        if (mapTileHandler == null)
        {
            Debug.LogError("No map tile placer found by mapstate");
        }
    }

}

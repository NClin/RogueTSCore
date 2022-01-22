using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNodeHandler : MonoBehaviour

{

    private MapState mapstate;

    [SerializeField]
    private GameObject resourceNodeBase;

    /// <summary>
    /// do not spawn node until it is added to resourceNodes in mapstate.
    /// </summary>
    ///
    public void AddResourceNode(Vector2Int tile, int stock, ResourceType resourceType)
    {
        GameObject toPlace = Instantiate(resourceNodeBase, new Vector3(tile.x, tile.y, 0), resourceNodeBase.transform.rotation);

        toPlace.AddComponent<ResourceNode>();
        toPlace.GetComponent<ResourceNode>().InitializeAs(stock, resourceType);
        mapstate.resourcesMap.AddNode(tile, toPlace.GetComponent<ResourceNode>());
    }


    private void Start()
    {
        mapstate = FindObjectOfType<MapState>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            var tile = VectorTools.GetClosestTileCoordinatesV2Int(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            AddResourceNode(tile, 100, ResourceType.Circle);
        }
    }

}

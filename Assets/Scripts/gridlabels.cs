using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;


public class gridlabels : MonoBehaviour
{
    private Tilemap tilemap;
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        if (tilemap != null)
        {
            foreach (Tile tile in tilemap.GetTilesBlock(tilemap.cellBounds))
            {
                Handles.color = Color.black;
                Handles.Label(transform.position, transform.position.x + ", " + transform.position.y);
            }
        }
    }
}

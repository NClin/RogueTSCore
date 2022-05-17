using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGridScript : MonoBehaviour
{
    [SerializeField]
    private GameObject squareShadedTile;

    private List<GameObject> placedTiles;

    [SerializeField]
    int size = 100;

    [SerializeField]
    bool on;

    bool active = false;

    private void Update()
    {
        if (on && !active)
        {
            TileTiles(size);
            active = true;
            on = true;
        }

    }

    private void TileTiles(int size)
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; x++)
            {
                var toPlace = Instantiate(squareShadedTile, transform);
                toPlace.transform.position = new Vector3(x, y, 0);
            }
        }
    }

}

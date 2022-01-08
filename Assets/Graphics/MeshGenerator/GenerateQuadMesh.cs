using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateQuads : MonoBehaviour
{
    [SerializeField]
    private int width = 10;
    [SerializeField]
    private float spacing = 1f;
    [SerializeField]
    private float maxHeight = 3f;

    private MeshFilter terrainMesh;

void Start()
    {
        if (terrainMesh == null)
        {

        }
    }

    float GetHeight(float x_coord, float z_coord)
    {
        return Mathf.Min(0, maxHeight - Vector2.Distance(Vector2.zero, new Vector2(x_coord, z_coord)));
    }
}

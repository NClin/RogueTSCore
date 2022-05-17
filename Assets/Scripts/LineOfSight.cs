using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    Texture2D LoSTexture;
    MeshRenderer LoSRenderer;

    private int width, height;
    bool[,] visionMap;

    bool initialized;

    Color inVision = new Vector4(0.3f,0.3f,1,0);

    public void Update()
    {
        if (!initialized)
        {
            return;
        }
        UpdateVisionShaderTexture();
    }

    public void InitializeLoS(int width, int height)
    {
        this.width = width;
        this.height = height;
        LoSTexture = new Texture2D(width, height);
        LoSTexture.filterMode = FilterMode.Point;
        visionMap = new bool[width, height];
        LoSRenderer = GetComponent<MeshRenderer>();



        UpdateVisionShaderTexture();
        initialized = true;


    }


    private void UpdateVisionShaderTexture()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                LoSTexture.SetPixel(x, y, Color.black);

                if (visionMap[x, y] == true)
                {
                    LoSTexture.SetPixel(x, y, inVision);
                }
            }
        }

        LoSTexture.Apply();

        LoSRenderer.material.SetTexture("_LoSTexture", LoSTexture);
    }

    public void AddVisionForFrame(Vector2Int center, float radius)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (Vector2.Distance(center, new Vector2Int(x,y)) < radius)
                {
                    visionMap[x, y] = true;
                }
            }
        }
    }

    public void SetScale(int width, int height)
    {
        float xOffset = width % 2 == 0 ? -0.5f : 0;
        float yOffset = height % 2 == 0 ? -0.5f : 0;

        transform.position = transform.position + new Vector3(width / 2 + xOffset, height / 2 + yOffset, 0);
        transform.localScale = new Vector3(width, height, 1);
    }
}

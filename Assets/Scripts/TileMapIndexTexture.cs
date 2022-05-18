using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapIndexTexture : MonoBehaviour
{
    public void GenerateFromTileTypeMap(TileTypeMap tileTypeMap)
    {
        int width = tileTypeMap.GetDimensions().x;
        int height = tileTypeMap.GetDimensions().y;

        var tex2d = GenerateTileIndexTexture(tileTypeMap, width, height);
        tex2d.filterMode = FilterMode.Point;
        UpdateIndexTextureAndDimensions(tex2d, width, height);
    }

    private void UpdateIndexTextureAndDimensions(Texture2D texture, int width, int height)
    {
        GetComponent<MeshRenderer>().material.SetTexture("_IndexTexture", texture);
        GetComponent<MeshRenderer>().material.SetVector("_IndexTextureDimensions", new Vector4(width, height, 0, 0));
        SetScale(width, height);    
    }

    public void SetScale(int width, int height)
    {
        float xOffset = width % 2 == 0 ? -0.5f : 0;
        float yOffset = height % 2 == 0 ? -0.5f : 0;

        transform.position = transform.position + new Vector3(width / 2 + xOffset, height / 2 + yOffset, 0);
        transform.localScale = new Vector3(width, height, 1);
    }

    /// <summary>
    /// TextureIndexes are encoded in 4-bit binary as a vector4.
    /// 
    ///             empty = 0;
    ///             wall = 1;
    ///             
    /// </summary>
    /// <param name="tileTypeMap"></param>
    /// <returns></returns>
    Texture2D GenerateTileIndexTexture(TileTypeMap tileTypeMap, int width, int height)
    {
        Texture2D indexTexture = new Texture2D(tileTypeMap.GetDimensions().x, tileTypeMap.GetDimensions().y);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int tocheck = new Vector2Int(x, y);

                if (tileTypeMap.GetTileType(tocheck) == TileType.empty)
                {
                    indexTexture.SetPixel(x, y, new Vector4(0, 0, 0, 0));
                }
                if (tileTypeMap.GetTileType(tocheck) == TileType.wall)
                {
                    indexTexture.SetPixel(x, y, new Vector4(0, 0, 0, 1));
                }
            }
        }

        indexTexture.Apply();

        return indexTexture;
    }

    /// <summary>
    /// Testing purposes.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    Texture2D GenerateRandomIndexTexture(int width, int height)
    {
        Texture2D indexTexture = new Texture2D(width, height);


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector4 index = new Vector4(0, 0, 1, 0);
                indexTexture.SetPixel(x, y, index);
            }
        }
        indexTexture.Apply();
        return indexTexture;
    }
}

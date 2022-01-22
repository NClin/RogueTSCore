using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoSTest : MonoBehaviour
{
    Texture2D LoSTexture;
    MeshRenderer LoSRenderer;

    private int width, height;

    bool initialized;

    Color inVision = new Vector4(0.3f, 0.3f, 1, 0);

    private void Start()
    {
        InitializeLoS(5, 5);
    }

    public void InitializeLoS(int width, int height)
    {
        LoSTexture = new Texture2D(width, height);
        LoSTexture.filterMode = FilterMode.Point;
        LoSTexture.wrapMode = TextureWrapMode.Clamp;
        LoSRenderer = GetComponent<MeshRenderer>();



        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x < 3)
                {

                    LoSTexture.SetPixel(x, y, Color.black);
                }
                else
                {
                    LoSTexture.SetPixel(x, y, Color.white);
                }
            }
        }
        
        LoSRenderer.material.EnableKeyword("_LoSTexture");

        MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
        LoSRenderer.GetPropertyBlock(_propBlock);
        _propBlock.SetTexture("_LoSTexture", LoSTexture);
        LoSRenderer.SetPropertyBlock(_propBlock);

        //SaveTexture(LoSTexture);
        //LoSRenderer.material.SetTexture("_LoSTexture", LoSTexture);
    }

    private IEnumerator UpdateVisionTextureAtEndOfFrame()
    {
        yield return new WaitForEndOfFrame();

        UpdateVisionShaderTexture();
    }

    private void SaveTexture(Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();
        var dirPath = Application.dataPath + "/RenderOutput";
        if (!System.IO.Directory.Exists(dirPath))
        {
            System.IO.Directory.CreateDirectory(dirPath);
        }
        System.IO.File.WriteAllBytes(dirPath + "/R_" + Random.Range(0, 100000) + ".png", bytes);
        Debug.Log(bytes.Length / 1024 + "Kb was saved as: " + dirPath);
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    private void UpdateVisionShaderTexture()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                LoSTexture.SetPixel(x, y, Color.black);

                //if (visionMap[x, y] == true)
                //{
                //    LoSTexture.SetPixel(x, y, inVision);
                //}

            }
        }


        MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
        LoSRenderer.GetPropertyBlock(_propBlock);
        _propBlock.SetTexture("_LoSTexture", LoSTexture);
        LoSRenderer.SetPropertyBlock(_propBlock);
    }
}

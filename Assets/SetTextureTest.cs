using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTextureTest : MonoBehaviour
{
    [SerializeField]
    Texture2D toSet;
    void Start()
    {
        toSet = new Texture2D(128, 128);

        //List<Color> colors = new List<Color>();
        
        for (int x = 0; x < 128; x++)
        {
            for (int y = 0; y < 128; y++)
            {
                toSet.SetPixel(x, y, Color.black);
            }
        }


        //toSet.SetPixels(0, 0, 40, 40, colors.ToArray());
        toSet.Apply();

        

        GetComponent<MeshRenderer>().material.SetTexture("_LoSTexture", toSet);
    }

}

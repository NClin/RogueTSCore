using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleLoSMesh : MonoBehaviour
{
    [SerializeField]
    private int width, height;

    public void SetScale()
    {
        float xOffset = width % 2 == 0 ? -0.5f : 0;
        float yOffset = height % 2 == 0 ? -0.5f : 0;

        transform.position = transform.position + new Vector3(width / 2 + xOffset, height / 2 + yOffset, 0);
        transform.localScale = new Vector3(width, height, 1);
    }
}

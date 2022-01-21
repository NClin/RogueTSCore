using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class AutomateShader : MonoBehaviour
{
    MeshRenderer[] renderTarget;
    int colorID;

    [SerializeField]
    Vector4 color;

    Vector4 _color;

    void Start()
    {
        _color = color;
        Debug.Log("STARTED");
        renderTarget = GetComponentsInChildren<MeshRenderer>();
        colorID = Shader.PropertyToID("_Color");

        DoColors();
    }

    void Update()
    {
        //if (_color != color)
        //{
        //    _color = color;
        //    DoColors();
        //}
    }

    void DoColors()
    {
        foreach (MeshRenderer target in renderTarget)
        {
            target.material.SetVector(colorID, color);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Unit unit;
    private Material mat;
    private MeshRenderer meshRenderer;
    private string propertyName;
    private int propID;

    void Start()
    {
        unit = GetComponentInParent<Unit>();
        propertyName = "_HealthPcnt";
        mat = GetComponent<MeshRenderer>().material;
        meshRenderer = GetComponent<MeshRenderer>();
        propID = Shader.PropertyToID(propertyName);
    }

    void Update()
    {
        if (unit == null) return;


        float pcntHealth = unit.GetHealthPercentage();

        mat.SetFloat(propID, pcntHealth);

        if (pcntHealth == 1) 
            meshRenderer.forceRenderingOff = true;
        else
            meshRenderer.forceRenderingOff = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SilexarsMaintainAspect : MonoBehaviour
{
    MeshRenderer meshRenderer;
    MaterialPropertyBlock _propBlock;

    [SerializeField]
    float cachedAspectRatio;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        _propBlock = new MaterialPropertyBlock();
        meshRenderer.GetPropertyBlock(_propBlock);

        cachedAspectRatio = transform.localScale.x / transform.localScale.y;
        _propBlock.SetFloat("_AspectRatio", cachedAspectRatio);
        meshRenderer.SetPropertyBlock(_propBlock);
    }

    private void Update()
    {
        float aspectRatio = transform.lossyScale.x  / transform.lossyScale.y;

        if (cachedAspectRatio != aspectRatio)
        {
            meshRenderer.GetPropertyBlock(_propBlock);
            _propBlock.SetFloat("_AspectRatio", aspectRatio);
            cachedAspectRatio = aspectRatio;
            meshRenderer.SetPropertyBlock(_propBlock);
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileGradientTwinkle : MonoBehaviour
{
    [SerializeField]
    Texture2D xGradient;
    MeshRenderer[] renderTargets;

    [SerializeField]
    Texture2D newTileTexture;

    int propID;

    [SerializeField]
    float transitionTime = 0.1f;

    [SerializeField]
    float borderWidth = 5;

    [SerializeField]
    float changeChance = 0.8f;

    [SerializeField]
    float frequency = 3;
    [SerializeField]
    float timeVariation = 1;
    float t = 0;

    Color currentColor;
    [SerializeField]
    Color targetColor;

    MaterialPropertyBlock _propBlock;


    List<int> rendererInstanceIDsWithRunningCoroutines;

    void Start()
    {
        renderTargets = GetComponentsInChildren<MeshRenderer>();
        propID = Shader.PropertyToID("_Color");
        rendererInstanceIDsWithRunningCoroutines = new List<int>();
        _propBlock = new MaterialPropertyBlock();
    }

    // using fixed so that it doesn't check time variation so often that it happens immediately, for e.g.
    void Update()
    {
        foreach (MeshRenderer renderer in renderTargets)
        {
            if (rendererInstanceIDsWithRunningCoroutines.Contains(renderer.GetInstanceID()))
            {
                continue;
            }

            // hack to test settings.
            Shader.PropertyToID("_MainTex");
            renderer.material.SetTexture("_MainTex", newTileTexture);

            renderer.material.SetFloat("_BorderWidth", borderWidth);


            float p = Random.Range(0f, 1f);

            if (p > changeChance)        
            {
                renderer.GetPropertyBlock(_propBlock);
                currentColor = _propBlock.GetColor("_Color");
                targetColor = xGradient.GetPixel(Random.Range(1, xGradient.width), 1);
                StartCoroutine(TransitionColor(currentColor, targetColor, transitionTime, renderer));
                    
            }
        }

    }

    IEnumerator TransitionColor(Color from, Color to, float time, MeshRenderer renderer)
    {
        float _t = 0;
        rendererInstanceIDsWithRunningCoroutines.Add(renderer.GetInstanceID());

        while (_t < time)
        {
            _t += Time.deltaTime;
            _propBlock.SetColor("_Color", Vector4.Lerp(from, to, _t / time));
            renderer.SetPropertyBlock(_propBlock);
            yield return null;
        }

        rendererInstanceIDsWithRunningCoroutines.Remove(renderer.GetInstanceID());
    }
}

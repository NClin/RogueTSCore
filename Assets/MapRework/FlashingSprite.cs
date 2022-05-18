using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FlashingSprite : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField]
    private float onDuration;
    [SerializeField]
    private float offDuration;

    private bool on;
    private float timeSinceChange;


    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        timeSinceChange += Time.deltaTime;

        if (on && timeSinceChange > onDuration)
        {
            sr.forceRenderingOff = on;
            on = !on;
            timeSinceChange = 0;
        }
        else if (!on && timeSinceChange > offDuration)
        {
            sr.forceRenderingOff = on;
            on = !on;
            timeSinceChange = 0;
        }

    }
}

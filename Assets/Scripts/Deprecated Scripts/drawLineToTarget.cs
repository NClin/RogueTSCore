using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TargetClosestModule))]
[RequireComponent(typeof(shapesDrawPulsingLine))]
public class drawLineToTarget : MonoBehaviour
{

    TargetClosestModule acquireTarget;
    shapesDrawPulsingLine drawPulsingLine;

    void Start()
    {
        acquireTarget = GetComponent<TargetClosestModule>();
        drawPulsingLine = GetComponent<shapesDrawPulsingLine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (acquireTarget.GetCurrentTarget() != null)
        {
            drawPulsingLine.DrawPulsingLine(transform.parent.position, acquireTarget.GetCurrentTarget().transform.position);
        }
    }
}

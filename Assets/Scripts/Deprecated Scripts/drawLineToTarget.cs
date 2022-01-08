using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HasTarget))]
[RequireComponent(typeof(shapesDrawPulsingLine))]
public class drawLineToTarget : MonoBehaviour
{

    HasTarget hasTarget;
    shapesDrawPulsingLine drawPulsingLine;

    void Start()
    {
        hasTarget = GetComponent<HasTarget>();
        drawPulsingLine = GetComponent<shapesDrawPulsingLine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasTarget.target != null)
        {
            drawPulsingLine.DrawPulsingLine(transform.parent.position, hasTarget.target.transform.position);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TargetClosestModule))]
public class MarkTarget : MonoBehaviour
{

    TargetClosestModule acquireTarget;

    private void Start()
    {
        acquireTarget = GetComponent<TargetClosestModule>();
    }

    private void Update()
    {
        if (acquireTarget.GetCurrentTarget() != null)
        {
            acquireTarget.GetCurrentTarget().GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasTarget : MonoBehaviour
{
    /// <summary>
    /// Targeting module should be liberal about clearing this. Only have a target while attacking.
    /// </summary>
    public GameObject? target;

    public bool hasTarget()
    {
        if (target != null)
        {
            return true;
        }
        
        return false;
    }

}

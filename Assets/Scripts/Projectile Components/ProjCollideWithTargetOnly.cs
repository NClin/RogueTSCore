using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HasTarget))]
[RequireComponent(typeof(ProjectileEffects))]
public class ProjCollideWithTargetOnly : MonoBehaviour
{
    public bool expended = false;

    private void OnTriggerEnter(Collider collider)
    {

        var target = GetComponent<HasTarget>().target;

        if (collider.gameObject == target
            && expended == false)
        {
            expended = true;
            GetComponent<ProjectileEffects>().DoEffects(target);
        }
    }
}

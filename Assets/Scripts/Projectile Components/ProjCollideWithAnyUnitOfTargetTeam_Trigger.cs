using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(HasTarget))]
[RequireComponent(typeof(ProjectileEffects))]
public class ProjCollideWithAnyUnitOfTargetTeam_Trigger : MonoBehaviour
{
    public bool expended = false;

    private void Start()
    {
        Debug.Log("trigger proj");
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Collided");

        var target = GetComponent<HasTarget>().target;
        if (target == null)
        {
            return;
        }

        if (target.GetComponent<Unit>() != null)
        {
            Team targetTeam = target.GetComponent<Unit>().team;

            Debug.Log("team: " + targetTeam);

            if (collider.gameObject.GetComponent<Unit>() != null)
            {
                Team colTeam = collider.gameObject.GetComponent<Unit>().team;

                Debug.Log("collision team: " + colTeam);

                if (colTeam == targetTeam
                    && expended == false)
                {
                    expended = true;
                    GetComponent<ProjectileEffects>().DoEffects(target);
                }
            }
        }
    }
}


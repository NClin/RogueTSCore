using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(HasTarget))]
[RequireComponent(typeof(ProjectileEffects))]
public class TargetTeamCollision : MonoBehaviour
{
    public bool expended = false;

    private void OnTriggerEnter(Collider collider)
    {
        var target = GetComponent<HasTarget>().target;

        if (target.GetComponent<Unit>() != null)
        {
            Team targetTeam = target.GetComponent<Unit>().team;

            if (collider.gameObject.GetComponent<Unit>() != null)
            {
                Team colTeam = collider.GetComponent<Unit>().team;

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

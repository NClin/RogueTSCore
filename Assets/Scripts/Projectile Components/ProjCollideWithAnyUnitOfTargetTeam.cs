using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(HasTarget))]
[RequireComponent(typeof(ProjectileEffects))]
public class ProjCollideWithAnyUnitOfTargetTeam : MonoBehaviour
{
    public bool expended = false;


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");

        var target = GetComponent<HasTarget>().target;
        if (target==null)
        {
            return;
        }

        if (target.GetComponent<Unit>() != null)
        {
            Team targetTeam = target.GetComponent<Unit>().team;

            Debug.Log("team: " + targetTeam);

            if (collision.gameObject.GetComponent<Unit>() != null)
            {
                Team colTeam = collision.gameObject.GetComponent<Unit>().team;

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

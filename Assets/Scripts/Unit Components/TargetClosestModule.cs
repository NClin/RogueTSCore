using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetClosestModule : MonoBehaviour
{

    [SerializeField]
    private float range;
    [SerializeField]
    private float scanFrequency;

    private GameObject? target;

    [SerializeField]
    private Team teamToTarget;

    private bool scanRunning;

    private void Update()
    {
        if (scanRunning == false)
        {
            StartCoroutine(targetSearchCoroutine());
        }
    }

    public GameObject? GetCurrentTarget()
    {
        return target;
    }

    private IEnumerator targetSearchCoroutine()
    {
        while (true)
        {
            scanRunning = true;
            if (target != null)
            {
                if (Vector2.Distance(transform.position, target.transform.position) > range)
                {
                    target = null;
                }
            }

            if (target == null)
            {
                FindClosestTarget(teamToTarget);
            }

            yield return new WaitForSeconds(scanFrequency);
        }
    }
    
    private void FindClosestTarget(Team targetTeam)
    {
        GameObject? closestTarget = null;
        float closestDistance = float.MaxValue;

        var potentials = Physics.OverlapSphere(transform.position, range);
        if (potentials.Length == 0)
        {
            target = null;
            return;
        }

        foreach (Collider potential in potentials)
        {

            if (potential.gameObject.GetComponent<Targetable>() != null
                && potential.gameObject != gameObject
                && potential.gameObject.GetComponent<UnitInfo>().team == targetTeam
                )
            {

                var d = Vector3.Distance(transform.position, potential.transform.position);
                if (d < closestDistance)
                {
                    closestDistance = d;
                    closestTarget = potential.gameObject;
                }
            }
        }

        if (closestTarget != null)
        {
            target = closestTarget;
        }
    }
}

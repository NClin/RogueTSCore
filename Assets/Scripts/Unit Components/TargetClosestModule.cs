using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class TargetClosestModule : MonoBehaviour
{

    [SerializeField]
    public float range;
    [SerializeField]
    private float scanFrequency = 1;

    private HasTarget hasTarget;

    [SerializeField]
    public Team teamToTarget;

    private bool scanRunning;

    private void Update()
    {
        if (hasTarget == null)
        {
            hasTarget = GetComponent<HasTarget>();
            if (hasTarget == null)
            {
                gameObject.AddComponent<HasTarget>();
            }
        }

        if (scanRunning == false)
        {
            StartCoroutine(targetSearchCoroutine());
        }
    }

    /// <summary>
    /// Legacy, do not use. Use HasTarget.target instead.
    /// </summary>
    /// <returns></returns>
    public GameObject? GetCurrentTarget()
    {
        return hasTarget.target;
    }

    private IEnumerator targetSearchCoroutine()
    {
        while (true)
        {
            scanRunning = true;

            if (GetCurrentTarget() == null)
            {
                yield return new WaitForSeconds(scanFrequency);
            }

            if (GetCurrentTarget() != null && Vector2.Distance(transform.position, hasTarget.transform.position) > range)
            {
                hasTarget.target = null;
            }

            FindClosestTarget(teamToTarget);

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
            hasTarget.target = null;
            return;
        }

        foreach (Collider potential in potentials)
        {

            if (potential.gameObject.GetComponent<Targetable>() != null
                && potential.gameObject != gameObject
                && potential.gameObject.GetComponent<Unit>().team == targetTeam
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
            hasTarget.target = closestTarget;

        }
        else
        {

        }
    }
}

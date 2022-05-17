using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetingMode
{
    Manual,
    Closest,
    ClosestTo,
    Point
}

[RequireComponent(typeof(HasTarget))]
public class TargetingModule : MonoBehaviour
{
    public Team toTarget;
    public TargetingMode targetingMode = TargetingMode.Closest;
    public int targetingRange;

    private Vector3 pointTarget;
    private float t = 0;
    private float frequency = 0.5f;



    void Update()
    {

        if (targetingMode == TargetingMode.Closest)
        {
            t += Time.deltaTime;
            if (t > frequency)
            {
                AutoClosestTarget();
                t = 0;
            }
            return;
        }
        if (targetingMode == TargetingMode.Manual)
        {
            // default to closest
            if (!GetComponent<HasTarget>().hasTarget())
            {
                targetingMode = TargetingMode.Closest;
            }
            return;
        }
        if (targetingMode == TargetingMode.ClosestTo)
        {
            t += Time.deltaTime;
            if (t > frequency)
            {
                SetClosestTarget(pointTarget, targetingRange);
                t = 0;
            }
        }
    }

    public void ManualTarget(GameObject target)
    {
        targetingMode = TargetingMode.Manual;
        GetComponent<HasTarget>().target = target;
    }

    /// <summary>
    /// not yet implemented
    /// </summary>
    /// <param name="targetPoint"></param>
    public void AtPoint(Vector3 targetPoint)
    {
        targetingMode = TargetingMode.Point;
        pointTarget = targetPoint;
        SetClosestTarget(pointTarget, targetingRange);
    }

    public void AutoClosestTarget()
    {
        SetClosestTarget(transform.position, targetingRange);
    }

    private void SetClosestTarget(Vector3 position, float range)
    {
        GameObject? closestTarget = null;
        float closestDistance = float.MaxValue;

        var potentials = Physics.OverlapSphere(position, range);
        if (potentials.Length == 0)
        {
            GetComponent<HasTarget>().target = null;
            return;
        }

        foreach (Collider potential in potentials)
        {

            if (potential.gameObject.GetComponent<Targetable>() != null
                && potential.gameObject != gameObject // not own parent object
                && potential.gameObject.GetComponent<Unit>().team == toTarget
                )
            {
                var d = Vector3.Distance(position, potential.transform.position);
                if (d < closestDistance)
                {
                    closestDistance = d;
                    closestTarget = potential.gameObject;
                }
            }
        }

        if (closestTarget != null)
        {
            GetComponent<HasTarget>().target = closestTarget;
        }
        else
        {
            GetComponent<HasTarget>().target = null;
        }

    }

}

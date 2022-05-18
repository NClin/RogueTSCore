using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class SeekBehaviour : MonoBehaviour
{

    [SerializeField]
    private float seekRange;
    [SerializeField]
    private float seekToDistance;
    [SerializeField]
    private Team seekTeam;


    private GameObject? seekTarget;

    [SerializeField]
    private Team teamToTarget;

    private bool scanRunning;
    private float scanFrequency = 1;

    private MovementStripped movement;
    private bool initialized;

    private void Start()
    {
        movement = GetComponentInParent<MovementStripped>();
        if (movement == null)
        {
            Debug.LogError("Could not find movement component in parent");
        }
    }

    private IEnumerator Initialize()
    {
        yield return new WaitForEndOfFrame();

        initialized = true;

    }

    private void Update()
    {
        if (initialized == false)
        {
            StartCoroutine(Initialize());
            return;
        }
        if (scanRunning == false)
        {
            StartCoroutine(targetSearchCoroutine());
        }

        // TODO: probably inefficient, spams move and stop orders
        if (seekTarget != null)
        {
            var dist = Vector3.Distance(transform.position, seekTarget.transform.position);
            
            if (dist > seekToDistance)
            {
                movement.MoveTo(seekTarget.transform.position);
            }   
            else
            {
                movement.StopOrder();
            }
        }
    }

    private IEnumerator targetSearchCoroutine()
    {
        while (true)
        {
            scanRunning = true;
            if (seekTarget != null)
            {
                if (Vector2.Distance(transform.position, seekTarget.transform.position) > seekRange)
                {
                    seekTarget = null;
                }
            }

            if (seekTarget == null)
            {
                FindClosestTarget();
            }

            yield return new WaitForSeconds(scanFrequency);
        }
    }

    private void FindClosestTarget()
    {
        GameObject? closestTarget = null;
        float closestDistance = float.MaxValue;

        var potentials = Physics.OverlapSphere(transform.position, seekRange);
        Debug.Log(potentials.Length);
        if (potentials.Length == 0)
        {
            seekTarget = null;
            return;
        }

        foreach (Collider potential in potentials)
        {
            Debug.Log(potential.gameObject.GetComponent<Unit>().team);
            if (potential.gameObject.GetComponent<Targetable>() != null
                && potential.gameObject != gameObject
                && potential.gameObject.GetComponent<Unit>().team == seekTeam)
            {
                var d = Vector3.Distance(transform.position, potential.transform.position);
                Debug.Log(d);
                if (d < closestDistance)
                {
                    closestDistance = d;
                    closestTarget = potential.gameObject;
                }
            }
        }

        if (closestTarget != null)
        {
            seekTarget = closestTarget;
        }
    }
}

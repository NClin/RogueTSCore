using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekProjBehaviour : MonoBehaviour
{
    private GameObject target;
    private Vector3 desiredVelocity;
    private float t;
    private Vector3 currentVelocity;
    private Vector3 steering;

    [SerializeField]
    private float speed = 3;
    [SerializeField]
    private float lifespan = 5;
    [SerializeField]
    private float maxSteering = 0.01f;



    void Start()
    {
        target = GetComponent<HasTarget>().target;
        currentVelocity = transform.up * speed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + currentVelocity);
    }

    void Update()
    {
        t += Time.deltaTime;
        if (t > lifespan)
        {
            Destroy(gameObject);
        }

        if (target == null)
        {
            desiredVelocity = transform.up * speed;
        }
        else
        {
            desiredVelocity =
                (target.transform.position
                - transform.position).normalized * speed;
            steering = (desiredVelocity - currentVelocity).normalized * maxSteering; // could be done in a smoother way.
        }

        var newVelocity = (currentVelocity + steering).normalized * speed * Time.deltaTime;
        transform.position += newVelocity;
        currentVelocity = newVelocity;

    }
}

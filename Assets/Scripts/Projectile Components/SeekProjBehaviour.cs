using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekProjBehaviour : MonoBehaviour
{
    private GameObject target;
    private Vector3 dir;
    private float t;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float lifespan;



    void Start()
    {
        target = GetComponent<HasTarget>().target;

        if (target == null)
        {
            dir = transform.forward;
        }
        else
        {
            dir = (target.transform.position - transform.position).normalized;
        }
    }

    void Update()
    {
        t += Time.deltaTime;
        if (t > lifespan)
        {
            Destroy(gameObject);
        }
        if (target != null)
        {
            dir = (target.transform.position - transform.position).normalized;
        }

        transform.position += dir * speed * Time.deltaTime;
    }
}

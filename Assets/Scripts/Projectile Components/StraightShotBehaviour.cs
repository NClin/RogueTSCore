using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightShotBehaviour : MonoBehaviour
{
    private ProjectileEffects effects;
    private GameObject target;
    private Vector3 dir;
    private bool expended = false;
    private float t;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float lifespan;



    void Start()
    {
        target = GetComponent<HasTarget>().target;
        effects = GetComponent<ProjectileEffects>();

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
        
        transform.position += dir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == target
            && expended == false)
        {
            expended = true;
            effects.DoEffects(target);
        }
    }
}

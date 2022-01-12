using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HasTarget))]
public class FireProjectileTowardsTarget : MonoBehaviour
{

    [SerializeField]
    public GameObject projectile;
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    public float cooldown;
    [SerializeField]
    public int damage;
    [SerializeField]
    private float angleVariability = 60f;

    private HasTarget hasTarget;
    float t = 0;



    private void Start()
    {
        hasTarget = GetComponent<HasTarget>();
        if (firePoint == null)
        {
            firePoint = transform;
        }
    }

    private void Update()
    {
        if (hasTarget == null)
        {
            hasTarget = GetComponent<HasTarget>();
        }

        t += Time.deltaTime;
        if (t > cooldown)
        {
            Fire();
            t = 0;
        }

    }

    void Fire()
    {
        if (hasTarget.target != null)
        {
            var fired = Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);
            //fired.transform.Rotate(0, 0, Random.Range(-angleVariability, angleVariability));
            fired.GetComponent<HasTarget>().target = hasTarget.target;
            fired.GetComponent<ProjectileEffects>().damage = damage;
        }
        else
        {
        }
    }
}

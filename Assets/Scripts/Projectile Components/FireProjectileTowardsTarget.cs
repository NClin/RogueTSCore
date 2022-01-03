using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectileTowardsTarget : MonoBehaviour
{

    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private float cooldown;
    [SerializeField]
    public int damage;

    private TargetClosestModule targetClosest;
    float t = 0;

    private void Start()
    {
        targetClosest = GetComponent<TargetClosestModule>();
    }

    private void Update()
    {
        t += Time.deltaTime;
        if (t > cooldown) Fire();
    }

    void Fire()
    {
        if (targetClosest.GetCurrentTarget() != null)
        {
            var fired = Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);
            fired.GetComponent<HasTarget>().target = targetClosest.GetCurrentTarget();
            fired.GetComponent<ProjectileEffects>().damage = damage;
            t = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnitNearby : MonoBehaviour
{
    [SerializeField]
    private GameObject unitToSpawn;
    [SerializeField]
    private int spawnMaxHealth;
    [SerializeField]
    private int spawnMaxShield;
    [SerializeField]
    private int spawnDamage;
    [SerializeField]
    private float spawnCooldown;

    private float t;

    void Update()
    {
        t += Time.deltaTime;
        if (t > spawnCooldown)
        {
            spawnUnit();
            t = 0;
        }
    }

    private void spawnUnit()
    {
        GameObject toSpawn = Instantiate(unitToSpawn, transform.position, Quaternion.identity);
        Unit toSpawnInfo = toSpawn.GetComponent<Unit>();
        toSpawnInfo.SetSpawnValues(spawnMaxHealth, spawnMaxShield);
        //TODO: Make attack stats generic
        unitToSpawn.GetComponentInChildren<FireProjectileTowardsTarget>().damage = spawnDamage;
        var randomDir = Random.insideUnitSphere.normalized;
        randomDir.z = 0;
        unitToSpawn.GetComponent<MovementStripped>().MoveTo(transform.position + randomDir);
    }
}

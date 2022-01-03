using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnitNearby : MonoBehaviour
{
    [SerializeField]
    private GameObject unitToSpawn;
    [SerializeField]
    private int spawnHealth;
    [SerializeField]
    private int spawnMaxHealth;
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
        UnitInfo toSpawnInfo = toSpawn.GetComponent<UnitInfo>();

        toSpawnInfo.health = spawnHealth;
        toSpawnInfo.maxHealth = spawnMaxHealth;
        //TODO: Make attack stats generic
        unitToSpawn.GetComponentInChildren<FireProjectileTowardsTarget>().damage = spawnDamage;
        var randomDir = Random.insideUnitSphere.normalized;
        randomDir.z = 0;
        unitToSpawn.GetComponent<Movement>().MoveTo(transform.position + randomDir);
    }
}

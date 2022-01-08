using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFactory : MonoBehaviour
{
    [SerializeField]
    private GameObject unitBase;

    /// <summary>
    /// Use this only after the spawn has been validated.
    /// </summary>
    /// <param name="spwnInfo"></param>
    /// <param name="selectable"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject SpawnUnit(UnitSpawnInfo spwnInfo, bool selectable, Vector3 position)
    {
        GameObject toSpawn = Instantiate(unitBase, position, Quaternion.identity);
        Unit toSpawnUnitInfo = toSpawn.GetComponent<Unit>();
        toSpawnUnitInfo.team = spwnInfo.team;

        if (selectable)
        {
            toSpawn.AddComponent<SelectableUnit>();
        }

        toSpawnUnitInfo.SetSpawnValues(spwnInfo.maxHealth, spwnInfo.maxShield);

        if (spwnInfo.range != 0)
        {
            var tgtmodule = toSpawn.AddComponent<TargetingModule>();
            tgtmodule.targetingRange = spwnInfo.range;
            tgtmodule.toTarget = spwnInfo.teamToTarget;
        }
        if (spwnInfo.projectileBase != null)
        {
            var fireModule = toSpawn.AddComponent<FireProjectileTowardsTarget>();
            fireModule.damage = spwnInfo.attackDamage;
            fireModule.cooldown = spwnInfo.attackCooldown;
            fireModule.projectile = spwnInfo.projectileBase;
        }

        return toSpawn;
    }

}

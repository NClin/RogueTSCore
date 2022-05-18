using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTarget : IPower
{
    public float cooldown { get; set; } = 7f;
    public PowerTypeEnum powerTypeEnum { get; set; } = PowerTypeEnum.targeted;
    public int maxRange { get; set; } = 4;
    public float timeSinceUse { get; set; } = float.MaxValue;
    public int healAmount = 25;

    private MapState mapState;
    
    public bool IsReady()
    {
        if (timeSinceUse > cooldown)
        {
            return true;
        }

        return false;
    }

    public void OnUpdate()
    {
        timeSinceUse += Time.deltaTime;
    }

    public void OnUse(Vector2Int targetTile, GameObject user)
    {
        if (!IsReady()) return;

        if (mapState == null) 
            mapState = GameObject.FindObjectOfType<MapState>();

        if (!mapState.unitMap.IsUnitAt(targetTile)) return;
        

        var targetUnit = mapState.unitMap.GetUnitAt(targetTile).GetComponent<Unit>();
        var userTeam = user.GetComponent<Unit>().team;
        if (targetUnit == null || targetUnit.team != userTeam) return;

        targetUnit.ChangeHealth(healAmount);
        timeSinceUse = 0;
    }

    public IPower Clone()
    {
        return (IPower)this.MemberwiseClone();
    }
}

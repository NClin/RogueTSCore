using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : IPower
{
    public float cooldown { get; set; } = 12f;
    public PowerTypeEnum powerTypeEnum { get; set; } = PowerTypeEnum.targeted;
    public int maxRange { get; set; } = 5;
    public float timeSinceUse { get; set; } = float.MaxValue;
    private MapState mapState;

    public void OnUse(Vector2Int targetTile, GameObject user)
    {
        if (mapState == null) mapState = GameObject.FindObjectOfType<MapState>();

        if (IsReady() && !mapState.unitMap.IsUnitAt(targetTile))
        { 
            user.GetComponent<MovementStripped>().SetPosition(targetTile);
            timeSinceUse = 0;
        }
    }

    public void OnUpdate()
    {
        timeSinceUse += Time.deltaTime;
    }

    public bool IsReady()
    {
        if (timeSinceUse > cooldown)
        {
            return true;
        }

        return false;
    }

    public IPower Clone()
    {
        return (IPower)this.MemberwiseClone();
    }
}

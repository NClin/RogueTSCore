using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : IPower
{
    public float cooldown { get; set; } = 2f;
    public PowerTypeEnum powerTypeEnum { get; set; } = PowerTypeEnum.targeted;
    public int maxRange { get; set; } = 10;
    public float timeSinceUse { get; set; } = float.MaxValue;

    public void OnUse(Vector2Int targetTile, GameObject user)
    {
        if (IsReady())
        { 
            user.GetComponent<MoveableUnit>().TeleportTo(targetTile);
            timeSinceUse = 0;
        }
    }

    public void OnUpdate()
    {
        timeSinceUse += Time.deltaTime;
    }

    private bool IsReady()
    {
        if (timeSinceUse > cooldown)
        {
            return true;
        }

        return false;
    }

}

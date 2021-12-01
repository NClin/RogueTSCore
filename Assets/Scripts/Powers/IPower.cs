using UnityEngine;

public interface IPower
{
    public void OnUpdate();
    public void OnUse(Vector2Int targetTile, GameObject user);
    public float cooldown { get; set; }
    public PowerTypeEnum powerTypeEnum { get; set; }
    public int maxRange { get; set; }
    public float timeSinceUse { get; set; }

}
using UnityEngine;

public interface IPower
{
    public abstract void OnUpdate();
    public abstract void OnUse(Vector2Int targetTile, GameObject user);
    public abstract bool IsReady();
    public float cooldown { get; set; }
    public PowerTypeEnum powerTypeEnum { get; set; }
    public int maxRange { get; set; }
    public float timeSinceUse { get; set; }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct UnitSpawnInfo
{
    public int maxHealth;
    public int maxShield;
    public Team team;
    public Sprite sprite;

    public int attackDamage;
    public float attackCooldown;
    public GameObject projectileBase;
    public int range;
    public Team teamToTarget;
    public IPower power;

    public int cost;
    public int dataCost;


}

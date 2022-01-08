using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    public int attackDamage { get; set; }
    public int attackRange { get; set; }
    public GameObject projectileBase { get; set; }
}

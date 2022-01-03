using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEffects : MonoBehaviour
{
    public int damage;

    public void DoEffects(GameObject affected)
    {
        Debug.Log("Hit!");
        affected.GetComponent<UnitInfo>().health -= damage;
        Destroy(gameObject);
    }

    public void DoEffects(Transform point)
    {

    }
}

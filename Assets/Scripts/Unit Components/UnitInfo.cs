using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    black,
    white
}

public class UnitInfo : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public Team team;

    private void Start()
    {
        SetTeamColor();
    }

    private void Update()
    {
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void SetTeamColor()
    {
        if (team == Team.black)
        {
            GetComponent<SpriteRenderer>().color = Color.black;
        }
        if (team == Team.white)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

}

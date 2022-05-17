using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    black,
    white
}

public class Unit : MonoBehaviour
{
    [SerializeField]
    private int health;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int shield;
    [SerializeField]
    private int maxShield;

    public Team team;
    private bool spawned = false;

    /// <summary>
    /// Can only be called once.
    /// </summary>
    /// <param name="spwnMaxHealth"></param>
    /// <param name="spwnMaxShield"></param>
    public void SetSpawnValues(int spwnMaxHealth, int spwnMaxShield)
    {
        if (!spawned)
        {
            maxHealth = spwnMaxHealth;
            ChangeHealth(spwnMaxHealth);
            maxShield = spwnMaxShield;
            ChangeShield(spwnMaxShield);
            spawned = true;
        }
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("took damage");

        if (shield > 0)
        {
            shield -= 1;
        }
        else
        {
            ChangeHealth(-amount);
        }
    }

    /// <summary>
    /// Use TakeDamage to deal damage.
    /// </summary>
    /// <param name="change"></param>
    public void ChangeHealth(int change)
    {
        health += change;
        CheckHealth();
    }

    public void ChangeShield(int amount)    
    {
        if (amount < 0)
        {
            Debug.LogError("cannot gain negative shield");
            return;
        }

        if (shield + amount > maxShield)
        {
            shield = maxShield;
        }
        else
        {
            shield += amount;
        }
    }

    public void ChangeMaxShield(int amount)
    {
        if (maxShield + amount <= 0)
        {
            maxShield = 0;
        }
        else
        {
            maxShield += amount;
        }
    }
    
    public float GetHealthPercentage()
    {
        return (float)health / (float)maxHealth;
    }

    private void Die()
    {
        if (GetComponent<Movement>() != null)
        {
            GetComponent<Movement>().Die();
        }
        
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

}

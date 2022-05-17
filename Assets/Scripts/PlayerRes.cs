using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRes : MonoBehaviour
{

    [SerializeField]
    private int mass;
    [SerializeField]
    private int data;
 
    /// <summary>
    /// Use for income and testing/debug. Use SpendMoney to attempt to spend money.
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeMass(int amount)
    {
        mass += amount;
        if (mass < 0) mass = 0;

    }

    public bool SpendMass(int amount)
    {
        if (mass >= amount)
        {
            ChangeMass(-amount);
            return true;
        }
        else return false;
    }

    /// <summary>
    /// Use for income and testing/debug. Use SpendData to attempt to spend money.
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeData(int amount)
    {
        data += amount;
        if (data < 0)
        {
            data = 0;
        }
    }

    public bool SpendData(int amount)
    {
        if (data >= amount)
        {
            ChangeData(-amount);
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetCurrentMass()
    {
        return mass;
    }

    public int GetCurrentData()
    {
        return data;
    }

}

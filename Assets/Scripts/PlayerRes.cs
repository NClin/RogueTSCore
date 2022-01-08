using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRes : MonoBehaviour
{

    [SerializeField]
    private int money;
    [SerializeField]
    private int data;
 
    /// <summary>
    /// Use for income and testing/debug. Use SpendMoney to attempt to spend money.
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeMoney(int amount)
    {
        money += amount;
        if (money < 0)
        {
            money = 0;
        }
    }

    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            ChangeMoney(-amount);
            return true;
        }
        else
        {
            return false;
        }
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

    public int GetCurrentMoney()
    {
        return money;
    }

}

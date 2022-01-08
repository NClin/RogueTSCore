using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [SerializeField]
    private int stock;

    public int ExtractStock(int amount)
    {
        if (amount > stock)
        {
            var toret = stock;
            stock -= stock;

            return toret;
        }
        else
        {
            stock -= amount;
            return amount;
        }
    }

    public bool SetStock(int totalStock)
    {
        stock = totalStock;
        return true;
    }

    public bool IsEmpty()
    {
        if (stock < 0)
        {
            stock = 0;
        }
        return stock == 0;
    }
}

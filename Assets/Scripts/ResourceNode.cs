using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [SerializeField]
    private int stock;
    [SerializeField]
    private ResourceType nodeResourceType;

    public int ExtractStock(int amount)
    {
        // returns 0 if empty.

        if (amount >= stock)
        {
            var remainingStock = stock;
            stock = 0;
            return remainingStock;
        }
        else
        {
            stock -= amount;
            return amount;
        }
    }

    public bool SetStock(int totalStock, ResourceType resourceType)
    {
        nodeResourceType = resourceType;
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

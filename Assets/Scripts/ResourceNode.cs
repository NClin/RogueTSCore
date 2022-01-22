using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [SerializeField]
    private int stock;
    [SerializeField]
    private ResourceType resourceType;

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

    public int GetStock()
    {
        return stock;
    }

    public ResourceType GetResourceType()
    {
        return resourceType;
    }

    public void SetStock(int totalStock, ResourceType resourceType)
    {
        this.resourceType = resourceType;
        stock = totalStock;
    }

    public bool IsEmpty()
    {
        if (stock < 0)
        {
            stock = 0;
        }
        return stock == 0;
    }

    public void InitializeAs(ResourceNode toAdd)
    {
        SetStock(toAdd.GetStock(), toAdd.GetResourceType());
    }

    public void InitializeAs(int stock, ResourceType resourceType)
    {
        SetStock(stock, resourceType);
    }
}

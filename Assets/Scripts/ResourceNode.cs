using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [SerializeField]
    private int stock;
    [SerializeField]
    private ResourceType resourceType;

    public bool BeingExtracted;

    bool posSet = false;
    private Vector2Int _position;
    public Vector2Int position { 
        get {
            return _position;
        } 
        set {
            if (!posSet)
            {
                _position = value;
                posSet = true;
            }
        }
    
    }

    public ResourceNode(int totalStock, Vector2Int position)
    {
        SetStock(totalStock);
        this.position = position;
    }

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

    public void SetStock(int totalStock)
    {
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
        SetStock(toAdd.GetStock());
    }

    public void InitializeAs(int stock, Vector2Int position)
    {
        SetStock(stock);
        this.position = position;
    }
}

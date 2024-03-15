using UnityEngine;
using System.Collections.Generic;
using System;

public class Inventory
{
    private Dictionary<ResourceSO, int> inventory = new();

    public Inventory() { inventory = new(); }

    public Inventory(List<ResourceSO> initialResources, List<int> initialResourceAmounts)
    {
        if (initialResources == null || initialResourceAmounts == null)
            return;

        if (initialResources.Count != initialResourceAmounts.Count)
            throw new ArgumentException("initialResouces count and initialResourceAmounts count must be the same.");

        for (int i = 0; i < initialResourceAmounts.Count; i++)
            inventory.Add(initialResources[i], initialResourceAmounts[i]);
    }

    public bool Add(ResourceSO resource, int amount)
    {
        if (inventory.TryGetValue(resource, out int _existingAmount))
        {
            inventory[resource] = amount + _existingAmount;
            return true;
        }

        else
        {
            inventory.Add(resource, amount);
            return false;
        }
    }

    public bool Add(ResourceSO resouce) => Add(resouce, 1);

    public int Remove(ResourceSO resource, int amount)
    {
        if (inventory.TryGetValue(resource, out int _existingAmount))
        {
            int _deltaAmount = _existingAmount - Mathf.Max(0, _existingAmount - amount);
            if (_existingAmount != _deltaAmount)
                inventory[resource] = _existingAmount - _deltaAmount;
            else inventory.Remove(resource);

            return _deltaAmount;
        }

        else return 0;
    }

    public int Remove(ResourceSO resource) => Remove(resource, 1);

    public bool Contains(ResourceSO resource) => inventory.ContainsKey(resource);

    public Dictionary<ResourceSO, int> GetInventory() => inventory;
}
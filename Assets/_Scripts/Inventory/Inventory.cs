using UnityEngine;
using System.Collections.Generic;
using System;

public class Inventory<T>
{
    public event Action<T, int> OnItemAdd;
    public event Action<T, int> OnItemRemove;

    private Dictionary<T, int> inventory = new();

    public Inventory() { inventory = new(); }

    public Inventory(List<T> initialResources, List<int> initialResourceAmounts)
    {
        if (initialResources == null || initialResourceAmounts == null)
            return;

        if (initialResources.Count != initialResourceAmounts.Count)
            throw new ArgumentException("initialResouces count and initialResourceAmounts count must be the same.");

        for (int i = 0; i < initialResourceAmounts.Count; i++)
            inventory.Add(initialResources[i], initialResourceAmounts[i]);
    }

    public bool Add(T _item, int _amount)
    {
        int _newAmount = _amount;
        bool _wasPresent = false;

        if (inventory.TryGetValue(_item, out int _existingAmount))
        {
            _newAmount += _existingAmount;
            _wasPresent = true;
        }

        inventory[_item] = _newAmount;
        OnItemAdd?.Invoke(_item, _newAmount);
        return _wasPresent;
    }

    public bool Add(T _item) => Add(_item, 1);

    public int Remove(T _item, int _amount)
    {
        if (!inventory.ContainsKey(_item))
        {
            OnItemRemove?.Invoke(_item, 0);
            return 0;
        }

        int _existingAmount = inventory[_item];
        int _deltaAmount = _existingAmount - Mathf.Max(0, _existingAmount - _amount);

        if (_existingAmount != _deltaAmount)
            inventory[_item] = _existingAmount - _deltaAmount;

        else
            inventory.Remove(_item);

        OnItemRemove?.Invoke(_item, _existingAmount - _deltaAmount);
        return _deltaAmount;
    }

    public int Remove(T _resource) => Remove(_resource, 1);

    public int Get(T _resource)
    {
        if (!inventory.ContainsKey(_resource))
            return -1;

        return inventory[_resource];
    }

    public bool Contains(T _resource) => inventory.ContainsKey(_resource);

    public Dictionary<T, int> GetInventory() => inventory;
}
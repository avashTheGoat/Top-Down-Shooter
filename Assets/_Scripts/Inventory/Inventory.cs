using UnityEngine;
using System.Collections.Generic;
using System;

public class Inventory<T> where T : Item
{
    public event Action<T, int> OnItemAdd;
    public event Action<T, int> OnItemRemove;

    private Dictionary<T, int> inventory = new();

    public Inventory() { inventory = new(); }

    public Inventory(List<T> _initialItems, List<int> _initialItemAmounts)
    {
        if (_initialItems == null || _initialItemAmounts == null)
            return;

        if (_initialItems.Count != _initialItemAmounts.Count)
            throw new ArgumentException("initialResouces count and initialResourceAmounts count must be the same.");

        for (int i = 0; i < _initialItemAmounts.Count; i++)
            inventory.Add(_initialItems[i], _initialItemAmounts[i]);
    }

    public Inventory(Dictionary<T, int> _initialInventory)
    {
        if (_initialInventory == null)
            throw new ArgumentNullException(nameof(_initialInventory));

        foreach (KeyValuePair<T, int> _itemAmount in _initialInventory)
            inventory.Add(_itemAmount.Key, _itemAmount.Value);
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

    public int Remove(T _item) => Remove(_item, 1);

    public int Get(T _item)
    {
        if (!inventory.ContainsKey(_item))
            return -1;

        return inventory[_item];
    }

    public bool Contains(T _item) => inventory.ContainsKey(_item);

    public Dictionary<T, int> GetDictionary() => inventory;

    public Inventory<T> Clone() => new(inventory);
}
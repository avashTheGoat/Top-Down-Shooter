using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerInventoryUI<T> where T : Item
{
    public PlayerInventoryUI(Image _inventorySlotPreab, IProvider<Inventory<T>> _inventoryProvider)
    {
        inventorySlotPrefab = _inventorySlotPreab;
        inventoryProvider = _inventoryProvider;
    }

    private Image inventorySlotPrefab;
    private IProvider<Inventory<T>> inventoryProvider;

    public void PopulateInventory(RectTransform _targetInventory)
    {
        foreach (KeyValuePair<T, int> _resourceCount in inventoryProvider.Provide().GetInventory())
        {
            Image _newInventorySlot = Object.Instantiate(inventorySlotPrefab, _targetInventory);
            _newInventorySlot.sprite = _resourceCount.Key.Image;
            _newInventorySlot.GetComponentInChildren<CounterUI>().SetCount(_resourceCount.Value);
        }
    }
}
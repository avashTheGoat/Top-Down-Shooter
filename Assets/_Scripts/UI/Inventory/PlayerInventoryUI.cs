using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerInventoryUI<T> where T : Item
{
    private ItemInventoryUI inventorySlotPrefab;
    private IProvider<Inventory<T>> inventoryProvider;
    private bool areSameItemsSeperated;

    public PlayerInventoryUI(ItemInventoryUI _inventorySlotPreab,
        IProvider<Inventory<T>> _inventoryProvider, bool _areSameItemsSeperated = false)
    {
        inventorySlotPrefab = _inventorySlotPreab;
        inventoryProvider = _inventoryProvider;
        areSameItemsSeperated = _areSameItemsSeperated;
    }

    public void PopulateInventory(RectTransform _targetInventory)
    {
        foreach (KeyValuePair<T, int> _resourceCount in inventoryProvider.Provide().GetDictionary())
        {
            if (!areSameItemsSeperated)
            {
                ItemInventoryUI _newInventorySlot = UnityEngine.Object.Instantiate(inventorySlotPrefab, _targetInventory);
                _newInventorySlot.ItemUI.ItemImage.sprite = _resourceCount.Key.Image;
                _newInventorySlot.CounterUI.SetCount(_resourceCount.Value);
            }

            else
            {
                for (int i = 0; i < _resourceCount.Value; i++)
                {
                    ItemInventoryUI _newInventorySlot = UnityEngine.Object.Instantiate(inventorySlotPrefab, _targetInventory);
                    _newInventorySlot.ItemUI.ItemImage.sprite = _resourceCount.Key.Image;
                }
            }
        }
    }

    public void UpdateInventory(RectTransform _targetInventory, Action<ItemInventoryUI, Item, int> _newItemAction = null,
        Action<ItemInventoryUI, Item, int> _updateValidItemAction = null)
    {
        if (!areSameItemsSeperated)
        {
            Inventory<T> _remainingItems = inventoryProvider.Provide().Clone();

            int _numChildren = _targetInventory.childCount;
            for (int i = 0; i < _numChildren; i++)
            {
                Transform _curChild = _targetInventory.GetChild(i);
                ItemInventoryUI _ui = _curChild.GetComponent<ItemInventoryUI>();

                T _castedItem = (T)_ui.ItemUI.Item;

                if (!_remainingItems.Contains(_castedItem))
                {
                    UnityEngine.Object.Destroy(_curChild.gameObject);
                    continue;
                }

                int _itemCount = _remainingItems.Get(_castedItem);
                _updateValidItemAction?.Invoke(_ui, _castedItem, _itemCount);

                _remainingItems.Remove(_castedItem, _itemCount);
            }

            foreach (KeyValuePair<T, int> _itemAmount in _remainingItems.GetDictionary())
            {
                ItemInventoryUI _ui = UnityEngine.Object.Instantiate(inventorySlotPrefab, _targetInventory);
                _newItemAction?.Invoke(_ui, _itemAmount.Key, _itemAmount.Value);
            }
        }
    
        else
        {
            Inventory<T> _remainingItems = inventoryProvider.Provide().Clone();

            int _numChildren = _targetInventory.childCount;
            for (int i = 0; i < _numChildren; i++)
            {
                Transform _curChild = _targetInventory.GetChild(i);
                ItemInventoryUI _ui = _curChild.GetComponent<ItemInventoryUI>();

                T _castedItem = (T)_ui.ItemUI.Item;

                if (!_remainingItems.Contains(_castedItem))
                {
                    UnityEngine.Object.Destroy(_curChild.gameObject);
                    continue;
                }

                _updateValidItemAction?.Invoke(_ui, _castedItem, 1);

                _remainingItems.Remove(_castedItem);
            }

            foreach (KeyValuePair<T, int> _itemAmount in _remainingItems.GetDictionary())
            {
                for (int i = 0; i < _itemAmount.Value; i++)
                {
                    ItemInventoryUI _ui = UnityEngine.Object.Instantiate(inventorySlotPrefab, _targetInventory);
                    _newItemAction?.Invoke(_ui, _itemAmount.Key, _itemAmount.Value);
                }
            }
        }
    }
}
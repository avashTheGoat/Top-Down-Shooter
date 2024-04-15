using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerInventoryUIManager : MonoBehaviour
{
    [SerializeField] private RectTransform inventoryItems;
    [SerializeField] private PlayerInventory playerInventory;

    [SerializeField] private Image inventorySlotPrefab;

    private void OnEnable()
    {
        foreach (KeyValuePair<ResourceSO, int> _resourceCount in playerInventory.ResourceInventory.GetInventory())
        {
            Image _newInventorySlot = Instantiate(inventorySlotPrefab, inventoryItems);
            _newInventorySlot.sprite = _resourceCount.Key.ResourceImage;
            _newInventorySlot.GetComponentInChildren<CounterUI>().SetCount(_resourceCount.Value);
        }
    }

    private void OnDisable()
    {
        inventoryItems.DestroyChildren();
    }
}
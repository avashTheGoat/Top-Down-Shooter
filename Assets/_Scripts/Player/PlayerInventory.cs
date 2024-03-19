using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    public Inventory Inventory { get; private set; } = new();

    [SerializeField] List<ResourceAmount> serializedInventoryRepresentation = new();

    private void Update()
    {
        serializedInventoryRepresentation = new();
        foreach (KeyValuePair<ResourceSO, int> _resourceAmount in Inventory.GetInventory())
            serializedInventoryRepresentation.Add(new ResourceAmount(_resourceAmount.Key, _resourceAmount.Value));
    }
}

[System.Serializable]
public struct ResourceAmount
{
    public ResourceSO Resource;
    public int Amount;

    public ResourceAmount(ResourceSO _resource, int _amount)
    {
        Resource = _resource;
        Amount = _amount;
    }
}
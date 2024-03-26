using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    public Inventory Inventory { get; private set; } = new();

    [SerializeField] private List<ResourceAmount> initialInventory = new();
    [SerializeField] private List<ResourceAmount> serializedInventoryRepresentation = new();

    private void Awake()
    {
        foreach (ResourceAmount _resourceAmount in initialInventory)
            Inventory.Add(_resourceAmount.Resource, _resourceAmount.Amount);
    }

    private void Update()
    {
        serializedInventoryRepresentation = new();
        foreach (KeyValuePair<ResourceSO, int> _resourceAmount in Inventory.GetInventory())
            serializedInventoryRepresentation.Add(new ResourceAmount(_resourceAmount.Key, _resourceAmount.Value));
    }
}
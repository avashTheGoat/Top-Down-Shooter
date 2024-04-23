using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour, IProvider<Inventory<Item>>, IProvider<Inventory<Resource>>
{
    public Inventory<Resource> ResourceInventory { get; private set; } = new();
    public Inventory<Item> ItemInventory { get; private set; } = new();

    [Header("Resource Inventory")]
    [SerializeField] private List<ResourceAmount> initialResourceInventory = new();
    [SerializeField] private List<ResourceAmount> serializedResourceInventoryRepresentation = new();
    [Space(15)]

    [Header("Item Inventory")]
    [SerializeField] private List<ItemAmount> initialItemInventory = new();
    [SerializeField] private List<ItemAmount> serializedItemInventoryRepresentation = new();

    private void Awake()
    {
        foreach (ResourceAmount _resourceAmount in initialResourceInventory)
            ResourceInventory.Add(_resourceAmount.Resource, _resourceAmount.Amount);

        foreach (ItemAmount _itemAmount in initialItemInventory)
            ItemInventory.Add(_itemAmount.Item, _itemAmount.Amount);
    }

    private void Update()
    {
        serializedResourceInventoryRepresentation = new();
        foreach (KeyValuePair<Resource, int> _resourceAmount in ResourceInventory.GetInventory())
            serializedResourceInventoryRepresentation.Add(new ResourceAmount(_resourceAmount.Key, _resourceAmount.Value));

        serializedItemInventoryRepresentation = new();
        foreach (KeyValuePair<Item, int> _resourceAmount in ItemInventory.GetInventory())
            serializedItemInventoryRepresentation.Add(new ItemAmount(_resourceAmount.Key, _resourceAmount.Value));
    }

    Inventory<Resource> IProvider<Inventory<Resource>>.Provide() => ResourceInventory;

    Inventory<Item> IProvider<Inventory<Item>>.Provide() => ItemInventory;
}
using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour, IProvider<Inventory<Item>>, IProvider<Inventory<Resource>>, IProvider<Inventory<WeaponMod>>
{
    public Inventory<Resource> ResourceInventory { get; private set; } = new();
    public Inventory<Item> ItemInventory { get; private set; } = new();
    public Inventory<WeaponMod> WeaponModInventory { get; private set; } = new();

    [Header("Resource Inventory")]
    [SerializeField] private List<ResourceAmount> initialResourceInventory = new();
    [SerializeField] private List<ResourceAmount> serializedResourceInventory = new();
    [Space(15)]

    [Header("Item Inventory")]
    [SerializeField] private List<ItemAmount> initialItemInventory = new();
    [SerializeField] private List<ItemAmount> serializedItemInventory = new();
    [Space(15)]

    [Header("Weapon Mod Inventory")]
    [SerializeField] private List<ItemAmount> initialWeaponModInventory = new();
    [SerializeField] private List<ItemAmount> serializedWeaponModInventory = new();

    private void Awake()
    {
        foreach (ResourceAmount _resourceAmount in initialResourceInventory)
            ResourceInventory.Add(_resourceAmount.Resource, _resourceAmount.Amount);

        foreach (ItemAmount _itemAmount in initialItemInventory)
            ItemInventory.Add(_itemAmount.Item, _itemAmount.Amount);

        foreach (ItemAmount _itemAmount in initialWeaponModInventory)
            WeaponModInventory.Add((WeaponMod)_itemAmount.Item, _itemAmount.Amount);
    }

    private void Update()
    {
        serializedResourceInventory = new();
        foreach (KeyValuePair<Resource, int> _resourceAmount in ResourceInventory.GetDictionary())
            serializedResourceInventory.Add(new ResourceAmount(_resourceAmount.Key, _resourceAmount.Value));

        serializedItemInventory = new();
        foreach (KeyValuePair<Item, int> _itemAmount in ItemInventory.GetDictionary())
            serializedItemInventory.Add(new ItemAmount(_itemAmount.Key, _itemAmount.Value));

        serializedWeaponModInventory = new();
        foreach (KeyValuePair<WeaponMod, int> _weaponModAmount in WeaponModInventory.GetDictionary())
            serializedWeaponModInventory.Add(new ItemAmount(_weaponModAmount.Key, _weaponModAmount.Value));
    }

    Inventory<Resource> IProvider<Inventory<Resource>>.Provide() => ResourceInventory;

    Inventory<Item> IProvider<Inventory<Item>>.Provide() => ItemInventory;

    Inventory<WeaponMod> IProvider<Inventory<WeaponMod>>.Provide() => WeaponModInventory;
}
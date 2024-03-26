using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerInventoryManager : MonoBehaviour
{
    public event Action WhileInventoryOpen;

    public event Action OnInventoryOpen;
    public event Action OnInventoryClose;

    [SerializeField] private KeyCode openInventoryKey;
    
    [Header("Inventory UI")]
    [SerializeField] private GameObject inventory;
    [SerializeField] private Button inventoryCloseButton;

    private void Start() => inventoryCloseButton.onClick.AddListener(() => SwitchInventory());

    private void Update()
    {
        if (Input.GetKeyDown(openInventoryKey))
            SwitchInventory();

        if (inventory.activeInHierarchy)
            WhileInventoryOpen?.Invoke();
    }

    private void SwitchInventory()
    {
        inventory.SetActive(!inventory.activeInHierarchy);
        if (inventory.activeInHierarchy)
            OnInventoryOpen?.Invoke();

        else OnInventoryClose?.Invoke();
    }
}
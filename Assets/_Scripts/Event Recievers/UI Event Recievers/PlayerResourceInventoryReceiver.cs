using UnityEngine;
using UnityEngine.UI;

public class PlayerResourceInventoryReceiver : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerInventoryManager playerInventoryManager;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private Component resourceInventoryProviderComponent;
    [Space(15)]

    [Header("UI")]
    [SerializeField] private RectTransform inventoryItems;
    [Space(15)]

    [Header("Prefabs")]
    [SerializeField] private Image itemSlotPrefab;

    private IProvider<Inventory<Resource>> inventoryProvider;
    private PlayerInventoryUI<Resource> playerInventoryUI;

    private void Awake()
    {
        inventoryProvider = (IProvider<Inventory<Resource>>)resourceInventoryProviderComponent;
        playerInventoryUI = new(itemSlotPrefab, inventoryProvider);
    }

    private void Start()
    {
        playerInventoryManager.WhileInventoryOpen += () =>
        {
            PlayerInteractionManager.EnableUiMode();
            PlayerInteractionManager.EnableMovement();
        };

        playerInventoryManager.OnInventoryOpen += () => playerInventoryUI.PopulateInventory(inventoryItems);

        playerInventoryManager.OnInventoryClose += () => PlayerInteractionManager.DisableUiMode();
        playerInventoryManager.OnInventoryClose += () => inventoryItems.DestroyChildren();
    }
}
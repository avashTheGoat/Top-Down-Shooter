using UnityEngine;

public class ResourceInventoryReceiver : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerInventoryManager playerInventoryManager;
    [SerializeField] private PlayerInventory playerInventory;
    [Space(15)]

    [Header("UI")]
    [SerializeField] private RectTransform inventoryItems;
    [Space(15)]

    [Header("Prefabs")]
    [SerializeField] private ItemInventoryUI itemSlotPrefab;

    private PlayerInventoryUI<Resource> playerInventoryUI;

    private void Awake() => playerInventoryUI = new(itemSlotPrefab, playerInventory);

    private void Start()
    {
        playerInventoryManager.WhileInventoryOpen += () =>
        {
            PlayerInteractionManager.EnableUiMode();
            PlayerInteractionManager.EnableMovement();
        };

        playerInventoryManager.OnInventoryOpen += () =>
        {
            playerInventoryUI.UpdateInventory(inventoryItems, (_ui, _item, _count) =>
            {
                _ui.ItemUI.SetItem(_item);
                _ui.CounterUI.SetCount(_count);
            });
        };

        playerInventoryManager.OnInventoryClose += () => PlayerInteractionManager.DisableUiMode();
        playerInventoryManager.OnInventoryClose += () => inventoryItems.DestroyChildren();
    }
}
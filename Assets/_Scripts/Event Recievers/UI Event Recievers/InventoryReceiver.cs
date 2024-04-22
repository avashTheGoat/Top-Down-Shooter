using UnityEngine;

public class InventoryReceiver : MonoBehaviour
{
    [SerializeField] private PlayerInventoryManager playerInventory;

    private void Start()
    {
        playerInventory.WhileInventoryOpen += () =>
        {
            PlayerInteractionManager.EnableUiMode();
            PlayerInteractionManager.EnableMovement();
        };

        playerInventory.OnInventoryClose += () => PlayerInteractionManager.DisableUiMode();
    }
}
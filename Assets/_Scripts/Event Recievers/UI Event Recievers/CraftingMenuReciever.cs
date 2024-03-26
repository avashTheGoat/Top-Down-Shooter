using UnityEngine;

public class CraftingMenuReciever : MonoBehaviour
{
    [SerializeField] private CraftingMenuManager craftingMenu;
    [SerializeField] private CraftingMenuUIManager craftingMenuUI;
    [SerializeField] private PlayerWeaponsManager playerWeapons;

    private void Start()
    {
        craftingMenu.WhileCraftingOpen += () => PlayerInteractionManager.EnableUiMode();
        craftingMenu.OnCraftingClose += () => PlayerInteractionManager.DisableUiMode();

        craftingMenuUI.OnCraft += _item =>
        {
            if (_item is Weapon)
                playerWeapons.AddWeapon((Weapon)_item);
        };
    }
}
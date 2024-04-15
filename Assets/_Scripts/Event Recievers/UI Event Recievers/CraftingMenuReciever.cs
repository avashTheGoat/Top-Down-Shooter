using UnityEngine;

public class CraftingMenuReciever : MonoBehaviour
{
    [SerializeField] private CraftingMenuManager craftingMenu;
    [SerializeField] private CraftingMenuUIManager craftingMenuUI;

    [Header("Player")]
    [SerializeField] private PlayerWeaponsManager playerWeapons;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private PlayerWeaponsSwitcher switcherForTestingWeaponMod;

    private void Start()
    {
        craftingMenu.WhileCraftingOpen += () => PlayerInteractionManager.EnableUiMode();
        craftingMenu.OnCraftingClose += () => PlayerInteractionManager.DisableUiMode();

        craftingMenuUI.OnCraft += _item =>
        {
            if (_item is Weapon _weapon)
                playerWeapons.AddWeapon(_weapon);

            else if (_item is WeaponMod _mod)
                _mod.ApplyMod(switcherForTestingWeaponMod.ActiveWeapon);
        };
    }
}
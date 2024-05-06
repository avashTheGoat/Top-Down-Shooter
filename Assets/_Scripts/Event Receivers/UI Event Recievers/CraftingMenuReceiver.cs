using UnityEngine;

public class CraftingMenuReceiver : MonoBehaviour
{
    [SerializeField] private MenuManager[] craftingMenu;
    [SerializeField] private CraftingMenuUIManager[] craftingMenuUI;

    [Header("Player")]
    [SerializeField] private PlayerWeaponsManager playerWeapons;
    [SerializeField] private PlayerInventory playerInventory;

    private void Awake()
    {
        if (craftingMenu.Length != craftingMenuUI.Length)
            throw new System.ArgumentException("The lengths of craftingMenu and craftingMenuUI are not the same.");
    }

    private void Start()
    {
        for (int i = 0; i < craftingMenu.Length; i++)
        {
            craftingMenu[i].WhileMenuOpen += () => PlayerInteractionManager.EnableUiMode();
            craftingMenu[i].OnMenuClose += () => PlayerInteractionManager.DisableUiMode();

            craftingMenuUI[i].OnCraft += _recipe =>
            {
                foreach (ResourceAmount _resourceAmount in _recipe.CraftingRequirements)
                    playerInventory.ResourceInventory.Remove(_resourceAmount.Resource, _resourceAmount.Amount);

                if (_recipe.Result is Weapon _weapon)
                    playerWeapons.AddWeapon(_weapon);

                else if (_recipe.Result is WeaponMod _mod)
                    playerInventory.WeaponModInventory.Add(_mod);
            };
        }
    }
}
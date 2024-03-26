using UnityEngine;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerWeaponsManager), typeof(PlayerWeaponsSwitcher))]
public class PlayerInteractionManager : MonoBehaviour
{
    private static PlayerMovement movement;
    private static PlayerWeaponsManager weaponsManager;
    private static PlayerWeaponsSwitcher weaponsSwitcher;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        weaponsManager = GetComponent<PlayerWeaponsManager>();
        weaponsSwitcher = GetComponent<PlayerWeaponsSwitcher>();
    }

    public static void DisableMovement() => movement.enabled = false;
    public static void DisableWeapons() => weaponsManager.PlayerWeapons.ForEach(_weapon => _weapon.enabled = false);
    public static void DisableWeaponSwitching() => weaponsSwitcher.enabled = false;
    public static void EnableCursor() => Cursor.visible = true;

    public static void EnableUiMode()
    {
        DisableMovement();
        DisableWeapons();
        DisableWeaponSwitching();
        EnableCursor();
    }

    public static void EnableMovement() => movement.enabled = true;
    public static void EnableWeapons() => weaponsManager.PlayerWeapons.ForEach(_weapon => _weapon.enabled = true);
    public static void EnableWeaponSwitching() => weaponsSwitcher.enabled = true;
    public static void DisableCursor() => Cursor.visible = false;

    public static void DisableUiMode()
    {
        EnableMovement();
        EnableWeapons();
        EnableWeaponSwitching();
        DisableCursor();
    }
}
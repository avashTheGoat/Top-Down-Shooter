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

    public static void SetMovement(bool _isEnabled)
    {
        if (movement == null)
            return;

        movement.enabled = _isEnabled;
    }

    public static void SetWeapons(bool _isEnabled)
    {
        if (weaponsManager == null)
            return;

        weaponsManager.PlayerWeapons.ForEach(_weapon => _weapon.enabled = _isEnabled);
    }

    public static void SetWeaponSwitching(bool _canSwitch)
    {
        if (weaponsSwitcher == null)
            return;

        weaponsSwitcher.enabled = _canSwitch;
    }

    public static void SetCursor(bool _isVisible) => Cursor.visible = _isVisible;

    public static void EnableUiMode()
    {
        SetMovement(false);
        SetWeapons(false);
        SetWeaponSwitching(false);
        SetCursor(true);
    }

    public static void DisableUiMode()
    {
        SetMovement(true);
        SetWeapons(true);
        SetWeaponSwitching(true);
        SetCursor(false);
    }
}
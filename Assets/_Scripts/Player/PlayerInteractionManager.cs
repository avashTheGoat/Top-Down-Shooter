using UnityEngine;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerWeaponsProvider), typeof(PlayerWeaponsSwitcher))]
public class PlayerInteractionManager : MonoBehaviour
{
    private static PlayerMovement movement;
    private static PlayerWeaponsProvider weaponsProvider;
    private static PlayerWeaponsSwitcher weaponsSwitcher;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        weaponsProvider = GetComponent<PlayerWeaponsProvider>();
        weaponsSwitcher = GetComponent<PlayerWeaponsSwitcher>();
    }

    public static void DisableMovement() => movement.enabled = false;
    public static void DisableWeapons() => weaponsProvider.GetWeapons<Weapon>().ForEach(_weapon => _weapon.enabled = false);
    public static void DisableWeaponSwitching() => weaponsSwitcher.enabled = false;

    public static void DisableMainInteraction()
    {
        DisableMovement();
        DisableWeapons();
        DisableWeaponSwitching();
    }

    public static void EnableMovement() => movement.enabled = true;
    public static void EnableWeapons() => weaponsProvider.GetWeapons<Weapon>().ForEach(_weapon => _weapon.enabled = true);
    public static void EnableWeaponSwitching() => weaponsSwitcher.enabled = true;

    public static void EnableMainInteraction()
    {
        EnableMovement();
        EnableWeapons();
        EnableWeaponSwitching();
    }
}
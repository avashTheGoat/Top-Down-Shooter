using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerWeaponsProvider))]
public class PlayerWeaponsSwitcher : MonoBehaviour
{
    [SerializeField] private KeyCode weaponSwitchKey;

    private List<Weapon> playerWeapons = new();
    private Weapon activeWeapon;
    private int curWeaponIndex = 0;

    private PlayerWeaponsProvider weaponsProvider;

    private void Awake()
    {
        weaponsProvider = GetComponent<PlayerWeaponsProvider>();
        playerWeapons = weaponsProvider.GetWeapons<Weapon>();

        foreach (Weapon _weapon in playerWeapons)
            _weapon.gameObject.SetActive(false);

        activeWeapon = playerWeapons[0];
        activeWeapon.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(weaponSwitchKey))
        {
            SwitchWeapon((curWeaponIndex + 1) % playerWeapons.Count);
            curWeaponIndex++;
        }
    }

    private void SwitchWeapon(int _weaponIndex)
    {
        activeWeapon.gameObject.SetActive(false);
        activeWeapon = playerWeapons[_weaponIndex];
        activeWeapon.gameObject.SetActive(true);
    }
}
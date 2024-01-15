using UnityEngine;
using System.Collections.Generic;

public class PlayerWeaponSwitcher : MonoBehaviour
{
    [SerializeField] private KeyCode weaponSwitchKey;
    [SerializeField] private List<Weapon> playerWeapons = new();

    private Weapon activeWeapon;
    private int curWeaponIndex = 0;

    private void Awake()
    {
        foreach (Weapon _weapon in playerWeapons)
        {
            _weapon.gameObject.SetActive(false);
        }

        activeWeapon = playerWeapons[0];
        activeWeapon.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(weaponSwitchKey))
        {
            SwitchPistol((curWeaponIndex + 1) % playerWeapons.Count);
            curWeaponIndex++;
        }
    }

    private void SwitchPistol(int _pistolIndex)
    {
        activeWeapon.gameObject.SetActive(false);
        activeWeapon = playerWeapons[_pistolIndex];
        activeWeapon.gameObject.SetActive(true);
    }
}
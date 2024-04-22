using UnityEngine;
using System;
using System.Collections.Generic;


public class ModMenuUIManager : MonoBehaviour
{
    public event Action<WeaponMod, Weapon> OnModAttach;
    public event Action<WeaponMod, Weapon> OnModDeattach;

    [Header("Dependencies")]
    [SerializeField] private PlayerWeaponsManager playerWeapons;
    [Space(15)]

    [Header("Prefabs")]
    [SerializeField] private ItemUI itemThumbnailPrefab;
    [SerializeField] private WeaponModsUI weaponModsPrefab;
    [Space(15)]

    [Header("Content Info")]
    [SerializeField] private RectTransform itemThumbnails;
    [SerializeField] private RectTransform weaponMods;

    private List<WeaponModsUI> weaponModsUIs = new();

    private void Awake() => UpdateWeapons();

    private void OnEnable() => UpdateWeapons();

    private void UpdateWeapons()
    {
        foreach (Weapon _weapon in playerWeapons.GetWeapons())
        {
            if (DoesWeaponUiExist(_weapon))
                continue;

            ItemUI _itemThumbnail = Instantiate(itemThumbnailPrefab, itemThumbnails);
            _itemThumbnail.ItemImage.sprite = _weapon.Image;

            WeaponModsUI _weaponMods = Instantiate(weaponModsPrefab, weaponMods);
            _weaponMods.SetWeapon(_weapon);

            weaponModsUIs.Add(_weaponMods);
        }
    }

    private bool DoesWeaponUiExist(Weapon _weapon)
    {
        foreach (WeaponModsUI _weaponModUI in weaponModsUIs)
        {
            if (ReferenceEquals(_weaponModUI.Weapon, _weapon))
                return true;
        }

        return false;
    }
}
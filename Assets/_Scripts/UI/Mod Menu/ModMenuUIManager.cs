using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class ModMenuUIManager : MonoBehaviour
{
    public event Action<WeaponMod, Weapon> OnModAttach;
    public event Action<WeaponMod, Weapon> OnModDeattach;

    [Header("Dependencies")]
    [SerializeField] private PlayerWeaponsManager playerWeapons;
    [SerializeField] private TabsUI tabsManager;
    [Space(15)]

    [Header("Prefabs")]
    [SerializeField] private ItemUI itemThumbnailPrefab;
    [SerializeField] private WeaponModsInfoUI weaponModsInfoPrefab;
    [Space(15)]

    [Header("Content Info")]
    [SerializeField] private RectTransform itemThumbnails;
    [SerializeField] private RectTransform weaponModsInfos;

    private List<WeaponModsInfoUI> weaponModsUIs = new();

    private void Awake() => UpdateWeapons();

    private void OnEnable() => UpdateWeapons();

    public WeaponModsInfoUI GetActiveWeaponModUI()
    {
        foreach (WeaponModsInfoUI _wepaonModUI in weaponModsUIs)
        {
            if (_wepaonModUI.isActiveAndEnabled)
                return _wepaonModUI;
        }

        return null;
    }

    private void UpdateWeapons()
    {
        foreach (Weapon _weapon in playerWeapons.GetWeapons())
        {
            if (DoesWeaponUiExist(_weapon))
                continue;

            ItemUI _itemThumbnail = Instantiate(itemThumbnailPrefab, itemThumbnails);
            _itemThumbnail.SetItem(_weapon);

            WeaponModsInfoUI _weaponMods = Instantiate(weaponModsInfoPrefab, weaponModsInfos);
            _weaponMods.SetWeapon(_weapon);

            Weapon _weaponClone = _weapon;
            foreach (ItemSlotUI _modUI in _weaponMods.AttachedModUIs)
            {
                _modUI.SlotUI.OnItemDropped += _droppedMod =>
                {
                    WeaponMod _mod = (WeaponMod)_droppedMod.GetComponent<ItemUI>().Item;
                    _modUI.SetItem(_mod);
                    OnModAttach?.Invoke(_mod, _weaponClone);
                };

                _modUI.SlotUI.OnItemRemoved += _removedMod =>
                {
                    _modUI.SetItem(null);
                    OnModDeattach?.Invoke((WeaponMod)_removedMod.GetComponent<ItemUI>().Item, _weaponClone);
                };
            }

            weaponModsUIs.Add(_weaponMods);
            tabsManager.AddTab(_weaponMods.gameObject, _itemThumbnail.GetComponent<Button>());
        }
    }

    private bool DoesWeaponUiExist(Weapon _weapon)
    {
        foreach (WeaponModsInfoUI _weaponModUI in weaponModsUIs)
        {
            if (ReferenceEquals(_weaponModUI.Weapon, _weapon))
                return true;
        }

        return false;
    }
}
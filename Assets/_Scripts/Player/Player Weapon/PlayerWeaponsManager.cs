using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerWeaponLogic), typeof(WeaponInitializer))]
public class PlayerWeaponsManager : MonoBehaviour, IProvider<Weapon>, IProvider<RangedWeapon>, IProvider<MeleeWeapon>
{
    [field: SerializeField] public List<Weapon> PlayerWeapons { get; private set; } = new();

    private PlayerWeaponLogic weaponLogic;
    private WeaponInitializer playerWeaponInitializer;

    private Transform trans;

    private void Awake()
    {
        trans = transform;

        weaponLogic = GetComponent<PlayerWeaponLogic>();
        playerWeaponInitializer = GetComponent<WeaponInitializer>();
    }

    private void Start()
    {
        playerWeaponInitializer.SetAttackAndReload(weaponLogic, weaponLogic);

        foreach (Weapon _playerWeapon in PlayerWeapons)
        {
            playerWeaponInitializer.ApplyWeaponTransformChange(_playerWeapon);
            playerWeaponInitializer.InitializeWeapon(_playerWeapon);
        }
    }

    public void AddWeapon(Weapon _weapon)
    {
        Weapon _weaponObject = Instantiate(_weapon, trans);
        playerWeaponInitializer.InitializeWeapon(_weaponObject);
        playerWeaponInitializer.ApplyWeaponTransformChange(_weaponObject);
        _weaponObject.gameObject.SetActive(false);

        PlayerWeapons.Add(_weaponObject);
    }

    public List<Weapon> GetWeapons() => ((IProvider<Weapon>)this).Provide();
    public List<RangedWeapon> GetRangedWeapons() => ((IProvider<RangedWeapon>)this).Provide();
    public List<MeleeWeapon> GetMeleeWeapons() => ((IProvider<MeleeWeapon>)this).Provide();

    List<Weapon> IProvider<Weapon>.Provide()
    {
        List<Weapon> _weapons = new();
        PlayerWeapons.ForEach(_weapon =>
        {
            if (_weapon == null)
                return;

            _weapons.Add(_weapon);
        });

        return _weapons;
    }

    List<MeleeWeapon> IProvider<MeleeWeapon>.Provide()
    {
        List<MeleeWeapon> _weapons = new();
        PlayerWeapons.ForEach(_weapon =>
        {
            if (_weapon == null)
                return;

            if (_weapon is MeleeWeapon)
                _weapons.Add(_weapon as MeleeWeapon);
        });

        return _weapons;
    }

    List<RangedWeapon> IProvider<RangedWeapon>.Provide()
    {
        List<RangedWeapon> _weapons = new();
        PlayerWeapons.ForEach(_weapon =>
        {
            if (_weapon == null)
                return;
            
            if (_weapon is RangedWeapon)
                _weapons.Add(_weapon as RangedWeapon);
        });

        return _weapons;
    }
}
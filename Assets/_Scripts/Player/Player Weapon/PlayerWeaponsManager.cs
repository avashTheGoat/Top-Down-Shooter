using UnityEngine;
using System.Collections.Generic;

public class PlayerWeaponsManager : MonoBehaviour, IProvider<List<Weapon>>, IProvider<List<RangedWeapon>>, IProvider<List<MeleeWeapon>>
{
    [field: SerializeField] public List<Weapon> PlayerWeapons { get; private set; } = new();

    [SerializeField] private PlayerWeaponLogic weaponLogic;
    [SerializeField] private WeaponInitializer playerWeaponInitializer;


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
            playerWeaponInitializer.InitializeTagsToIgnore(_playerWeapon);
        }
    }

    public void AddWeapon(Weapon _weapon)
    {
        Weapon _weaponObject = Instantiate(_weapon, trans);
        playerWeaponInitializer.ApplyWeaponTransformChange(_weaponObject);
        playerWeaponInitializer.InitializeWeapon(_weaponObject);
        playerWeaponInitializer.InitializeTagsToIgnore(_weaponObject);
        _weaponObject.gameObject.SetActive(false);

        PlayerWeapons.Add(_weaponObject);
    }

    public List<Weapon> GetWeapons() => ((IProvider<List<Weapon>>)this).Provide();
    public List<RangedWeapon> GetRangedWeapons() => ((IProvider<List<RangedWeapon>>)this).Provide();
    public List<MeleeWeapon> GetMeleeWeapons() => ((IProvider<List<MeleeWeapon>>)this).Provide();

    List<Weapon> IProvider<List<Weapon>>.Provide()
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

    List<MeleeWeapon> IProvider<List<MeleeWeapon>>.Provide()
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

    List<RangedWeapon> IProvider<List<RangedWeapon>>.Provide()
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
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponsProvider : MonoBehaviour, IProvider<List<Weapon>>, IProvider<List<RangedWeapon>>, IProvider<List<MeleeWeapon>>
{
    public List<Weapon> GetWeapons() => ((IProvider<List<Weapon>>) this).Provide();
    public List<RangedWeapon> GetRangedWeapons => ((IProvider<List<RangedWeapon>>) this).Provide();
    public List<MeleeWeapon> GetMeleeWeapons => ((IProvider<List<MeleeWeapon>>) this).Provide();

    [SerializeField] private Component enemiesProviderComponent;

    private IProvider<List<GameObject>> enemiesProvider;

    private void Awake()
    {
        if (enemiesProviderComponent is not IProvider<List<GameObject>>)
            Debug.LogError("Provided component must be a provider of a List of GameObjects.");

        enemiesProvider = (IProvider<List<GameObject>>)enemiesProviderComponent;
    }

    List<Weapon> IProvider<List<Weapon>>.Provide()
    {
        List<Weapon> _weapons = new();
        enemiesProvider.Provide().ForEach(_enemy =>
        {
            Weapon _weapon = _enemy.GetComponent<EnemyWeaponManager>().Weapon;
            if (_weapon == null)
                return;

            if (_weapon is Weapon)
                _weapons.Add(_weapon);
        });

        return _weapons;
    }

    List<MeleeWeapon> IProvider<List<MeleeWeapon>>.Provide()
    {
        List<MeleeWeapon> _weapons = new();
        enemiesProvider.Provide().ForEach(_enemy =>
        {
            Weapon _weapon = _enemy.GetComponent<EnemyWeaponManager>().Weapon;
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
        enemiesProvider.Provide().ForEach(_enemy =>
        {
            Weapon _weapon = _enemy.GetComponent<EnemyWeaponManager>().Weapon;
            if (_weapon == null)
                return;

            if (_weapon is RangedWeapon)
                _weapons.Add(_weapon as RangedWeapon);
        });

        return _weapons;
    }
}
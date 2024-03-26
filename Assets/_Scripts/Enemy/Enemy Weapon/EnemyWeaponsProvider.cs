using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponsProvider : MonoBehaviour, IProvider<Weapon>, IProvider<RangedWeapon>, IProvider<MeleeWeapon>
{
    [SerializeField] private EnemyWavesSpawner enemyWaves;

    public List<Weapon> GetWeapons() => ((IProvider<Weapon>) this).Provide();
    public List<RangedWeapon> GetRangedWeapons => ((IProvider<RangedWeapon>) this).Provide();
    public List<MeleeWeapon> GetMeleeWeapons => ((IProvider<MeleeWeapon>) this).Provide();


    List<Weapon> IProvider<Weapon>.Provide()
    {
        List<Weapon> _weapons = new();
        enemyWaves.SpawnedEnemies.ForEach(_enemy =>
        {
            Weapon _weapon = _enemy.GetComponent<EnemyWeaponManager>().Weapon;
            if (_weapon == null)
                return;

            if (_weapon is Weapon)
                _weapons.Add(_weapon);
        });

        return _weapons;
    }

    List<MeleeWeapon> IProvider<MeleeWeapon>.Provide()
    {
        List<MeleeWeapon> _weapons = new();
        enemyWaves.SpawnedEnemies.ForEach(_enemy =>
        {
            Weapon _weapon = _enemy.GetComponent<EnemyWeaponManager>().Weapon;
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
        enemyWaves.SpawnedEnemies.ForEach(_enemy =>
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
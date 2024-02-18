using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponsProvider : MonoBehaviour, IWeaponProvider
{
    [SerializeField] private EnemyWavesController enemyWaves;

    public List<T> GetWeapons<T>() where T : Weapon
    {
        List<T> _weapons = new();

        enemyWaves.SpawnedEnemies.ForEach((_enemy) =>
        {
            Component _weapon = _enemy.GetComponentInChildren(typeof(T), true);
            if (_weapon != null) _weapons.Add(_weapon as T);
        });

        return _weapons;
    }
}
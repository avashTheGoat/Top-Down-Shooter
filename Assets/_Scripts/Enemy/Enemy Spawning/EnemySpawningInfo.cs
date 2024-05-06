using System;
using UnityEngine;

[System.Serializable]
public struct EnemySpawningInfo
{
    #region Accessors
    [HideInInspector] public GameObject EnemyPrefab => enemyPrefab;
    [HideInInspector] public GameObject WeaponPrefab => weaponPrefab;
    
    [HideInInspector] public int SpawnChance => spawnChance;

    [HideInInspector] public bool IsBoss => isBoss;
    [HideInInspector] public string BossName => bossName;

    [HideInInspector] public bool IsRequired => isRequired;
    [HideInInspector] public bool IsOnlyEnemyOfType => isOnlyEnemyOfType;
    #endregion

    [Header("Prefabs")]
    [SerializeField] private GameObject enemyPrefab;
    [Tooltip("The weapon the enemy will be spawned with.")]
    [SerializeField] private GameObject weaponPrefab;
    [Space(15)]

    [Min(0)]
    [Tooltip("A value from 0 to 100 indicating the percent chance of the enemy spawning.")]
    [SerializeField] private int spawnChance;

    [Header("Boss Settings")]
    [SerializeField] private bool isBoss;
    [SerializeField] private string bossName;
    [Space(15)]

    [Header("Other Settings")]
    [SerializeField] private bool isRequired;
    [SerializeField] private bool isOnlyEnemyOfType;

    public void SetSpawnChance(int _newSpawnChance)
    {
        if (_newSpawnChance <= 0)
            throw new ArgumentOutOfRangeException(nameof(_newSpawnChance));

        spawnChance = _newSpawnChance;
        MonoBehaviour.print("Set to " + _newSpawnChance);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(enemyPrefab, weaponPrefab, spawnChance, isBoss, bossName, isRequired, isOnlyEnemyOfType);
    }

    public override bool Equals(object _obj)
    {
        EnemySpawningInfo _info = (EnemySpawningInfo)_obj;
        if (!Equals(_info.EnemyPrefab, enemyPrefab) || !Equals(_info.weaponPrefab, weaponPrefab))
            return false;

        return _info.IsBoss == isBoss && _info.IsOnlyEnemyOfType == _info.IsOnlyEnemyOfType && _info.IsRequired == isRequired;
    }

    public EnemySpawningInfo Copy()
    {
        EnemySpawningInfo _new = new();
        _new.enemyPrefab = enemyPrefab;
        _new.weaponPrefab = weaponPrefab;
        _new.spawnChance = spawnChance;
        _new.isBoss = isBoss;
        _new.bossName = bossName;
        _new.isRequired = isRequired;
        _new.isOnlyEnemyOfType = isOnlyEnemyOfType;

        return _new;
    }
}
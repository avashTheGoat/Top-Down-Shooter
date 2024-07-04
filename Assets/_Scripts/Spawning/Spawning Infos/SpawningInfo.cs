using System;
using UnityEngine;

[System.Serializable]
public struct SpawningInfo
{
    #region Accessors
    [HideInInspector] public GameObject ObjectPrefab => objectPrefab;

    [HideInInspector] public int SpawnChance => spawnChance;

    [HideInInspector] public bool IsBoss => isBoss;
    [HideInInspector] public string BossName => bossName;

    [HideInInspector] public bool IsRequired => isRequired;
    [HideInInspector] public bool IsOnlyEnemyOfType => isOnlyEnemyOfType;
    #endregion

    [Header("Prefabs")]
    [SerializeField] private GameObject objectPrefab;
    [Space(15)]

    [Header("Spawn Settings")]
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

    public SpawningInfo(GameObject _enemyPrefab, int _spawnChance = 100)
    {
        objectPrefab = _enemyPrefab;
        spawnChance = _spawnChance;
        isBoss = false;
        bossName = "";
        isRequired = false;
        isOnlyEnemyOfType = false;
    }

    public void SetSpawnChance(int _newSpawnChance)
    {
        if (_newSpawnChance <= 0)
            throw new ArgumentOutOfRangeException(nameof(_newSpawnChance));

        spawnChance = _newSpawnChance;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(objectPrefab, spawnChance, isBoss, bossName, isRequired, isOnlyEnemyOfType);
    }

    public override bool Equals(object _obj)
    {
        SpawningInfo _info = (SpawningInfo)_obj;
        if (!Equals(_info.ObjectPrefab, objectPrefab))
            return false;

        return _info.IsBoss == isBoss && _info.IsOnlyEnemyOfType == _info.IsOnlyEnemyOfType && _info.IsRequired == isRequired;
    }

    public SpawningInfo Copy()
    {
        SpawningInfo _new = new(objectPrefab);
        _new.spawnChance = spawnChance;
        _new.isBoss = isBoss;
        _new.bossName = bossName;
        _new.isRequired = isRequired;
        _new.isOnlyEnemyOfType = isOnlyEnemyOfType;

        return _new;
    }
}
using UnityEngine;

[System.Serializable]
public struct EnemySpawningInfo
{
    [HideInInspector]
    public int SpawnChance => spawnChance;
    [HideInInspector]
    public GameObject EnemyPrefab => enemyPrefab;
    [HideInInspector]
    public GameObject WeaponPrefab => weaponPrefab;

    [SerializeField] private GameObject enemyPrefab;
    [Tooltip("The weapon the enemy will be spawned with.")]
    [SerializeField] private GameObject weaponPrefab;

    [Min(0)]
    [Tooltip("A value from 0 to 100 indicating the percent chance of the enemy spawning.")]
    [SerializeField] private int spawnChance;
}
using System.Collections.Generic;
using UnityEngine;
using NavMeshPlus.Components;
using System;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private NavMeshSurface spawnableSurface;
    
    [Header("Spawning Restrictions")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    [Space(15)]

    [Header("Player Spawning Restriction")]
    [SerializeField] private Transform player;
    [Tooltip("The minimum distance from the player a point needs to be to be a valid spawn location")]
    [SerializeField] private float minDistanceFromPlayerToSpawn;
    [Space(15)]

    [Header("Spawning Attempts")]
    [SerializeField] private int maxSpawnAttempts;
    [SerializeField] private Vector2[] worstCaseSpawnLocations;

    public List<GameObject> SpawnEnemies(EnemySpawningInfo[] _enemies, int _numEnemies)
    {
        if (_enemies == null)
        {
            throw new ArgumentNullException(nameof(_enemies), "The enemies spawning info array should not be null.");
        }

        if (_enemies.Length == 0)
        {
            throw new ArgumentException("The enemies spawning info array should not have a length of 0.", nameof(_enemies));
        }

        // validating EnemySpawingInfo[] parameter
        // may move this into main for loop to speed up spawning if required
        int _spawnChancesSum = 0;
        for (int i = 0; i < _enemies.Length; i++)
        {
            if (_enemies[i].EnemyPrefab == null)
                throw new ArgumentNullException("One of the EnemySpawningInfo struct's EnemyPrefab " +
                    "in the given EnemySpawningInfo array was null.");

            if (_enemies[i].WeaponPrefab == null)
                throw new ArgumentNullException("One of the EnemySpawningInfo struct's WeaponPrefab " +
                    "in the given EnemySpawningInfo array was null.");

            if (!_enemies[i].WeaponPrefab.TryGetComponent(out Weapon _))
                throw new ArgumentException("One of the EnemySpawningInfo struct's WeaponPrefab " +
                    "did not have a Weapon component attached to it.");
            if (!_enemies[i].EnemyPrefab.TryGetComponent(out EnemyStateMachine _))
                throw new ArgumentException("One of the EnemySpawningInfo struct's EnemyPrefab " +
                    "did not have an EnemyStateMachine component attached to it, so it is likely not an enemy.");

            _spawnChancesSum += _enemies[i].SpawnChance;
        }

        if (_spawnChancesSum != 100)
        {
            throw new ArgumentException("The total spawn chances for the EnemySpawningInfo[] parameter should be 100, " +
                $"but it was {_spawnChancesSum}.");
        }

        List<GameObject> _spawnedEnemies = new();
        for (int i = 0; i < _numEnemies; i++)
        {
            Vector2 _spawnLocation = GetRandomSpawnPosition();
            EnemySpawningInfo _enemySpawningInfo = GetSpawningInfo(UnityEngine.Random.Range(0, 101), _enemies);

            GameObject _enemy = Instantiate(_enemySpawningInfo.EnemyPrefab);
            Transform _enemyTransform = _enemy.transform;
            _enemyTransform.position = _spawnLocation;

            GameObject _weapon = Instantiate(_enemySpawningInfo.WeaponPrefab, _enemyTransform);
            _weapon.transform.rotation = Quaternion.identity;
            _weapon.SetActive(true);

            _enemy.GetComponent<EnemyStateMachine>().Init(_weapon.GetComponent<Weapon>());

            _spawnedEnemies.Add(_enemy);
        }

        return _spawnedEnemies;
    }

    private Vector2 GetRandomSpawnPosition()
    {
        Vector2 _spawnLocation = new Vector2(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY));
        _spawnLocation = spawnableSurface.navMeshData.sourceBounds.ClosestPoint(_spawnLocation);

        int _spawnAttempts = 1;
        while (_spawnAttempts < maxSpawnAttempts && Vector2.Distance(player.position, _spawnLocation) < minDistanceFromPlayerToSpawn)
        {
            _spawnLocation = new Vector2(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY));
            _spawnLocation = spawnableSurface.navMeshData.sourceBounds.ClosestPoint(_spawnLocation);

            _spawnAttempts++;
        }

        // if too close to player despite trying other spawn locations
        if (Vector2.Distance(player.position, _spawnLocation) < minDistanceFromPlayerToSpawn)
            _spawnLocation = worstCaseSpawnLocations[UnityEngine.Random.Range(0, worstCaseSpawnLocations.Length)];

        return _spawnLocation;
    }

    private EnemySpawningInfo GetSpawningInfo(int _spawnSeed, EnemySpawningInfo[] _enemies)
    {
        int _min = 0;
        EnemySpawningInfo _enemySpawningInfo = _enemies[0];
        for (int i = 0; i < _enemies.Length; i++)
        {
            if (_spawnSeed > _min && _spawnSeed < _min + _enemies[i].SpawnChance)
            {
                _enemySpawningInfo = _enemies[i];
                break;
            }

            _min += _enemies[i].SpawnChance;
        }

        return _enemySpawningInfo;
    }
}
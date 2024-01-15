using System.Collections.Generic;
using UnityEngine;
using NavMeshPlus.Components;

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
    [SerializeField] private float minDistanceFromPlayer;
    [Space(15)]

    [Header("Spawning Attempts")]
    [SerializeField] private int maxSpawnAttempts;
    [SerializeField] private Vector2[] worstCaseSpawnLocations;

    public List<GameObject> SpawnEnemies(EnemySpawningInfo[] _enemies, int _numEnemies)
    {
        if (_enemies is null)
        {
            Debug.LogError("The enemies spawning info array should not be null.");
        }

        if (_enemies.Length == 0)
        {
            Debug.LogError("The enemies spawning info array should not have a length of 0.");
        }

        List<GameObject> _spawnedEnemies = new();
        for (int i = 0; i < _numEnemies; i++)
        {
            Vector2 _spawnLocation = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            _spawnLocation = spawnableSurface.navMeshData.sourceBounds.ClosestPoint(_spawnLocation);

            int _spawnAttempts = 1;

            while (_spawnAttempts < maxSpawnAttempts && Vector2.Distance(player.position, _spawnLocation) < minDistanceFromPlayer)
            {
                _spawnLocation = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                _spawnLocation = spawnableSurface.navMeshData.sourceBounds.ClosestPoint(_spawnLocation);

                _spawnAttempts++;
            }

            // if too close to player despite trying other spawn locations
            if (Vector2.Distance(player.position, _spawnLocation) < minDistanceFromPlayer)
                _spawnLocation = worstCaseSpawnLocations[Random.Range(0, worstCaseSpawnLocations.Length)];

            int _enemyToSpawnSeed = Random.Range(0, 101);

            int _min = 0;
            GameObject _enemyToSpawn = _enemies[0].EnemyPrefab;
            EnemySpawningInfo _enemySpawningInfo = _enemies[0];
            for (int j = 0; j < _enemies.Length; j++)
            {
                if (_enemyToSpawnSeed > _min && _enemyToSpawnSeed < _min + _enemies[j].SpawnChance)
                {
                    _enemySpawningInfo = _enemies[j];
                    _enemyToSpawn = _enemySpawningInfo.EnemyPrefab;
                    break;
                }

                _min += _enemies[j].SpawnChance;
            }

            GameObject _enemy = Instantiate(_enemyToSpawn);
            Transform _enemyTransform = _enemy.transform;
            _enemyTransform.position = _spawnLocation;

            GameObject _weapon = Instantiate(_enemySpawningInfo.WeaponPrefab, _enemyTransform);
            _weapon.transform.rotation = Quaternion.identity;
            _weapon.SetActive(true);

            _spawnedEnemies.Add(_enemy);
        }

        return _spawnedEnemies;
    }
}
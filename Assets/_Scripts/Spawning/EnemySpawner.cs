using UnityEngine;
using System;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning Restrictions")]
    [Tooltip("The minimum distance from the player a point needs to be to be a valid spawn location")]
    [SerializeField] private float minDistanceFromPlayerToSpawn;
    [SerializeField] private Vector2 bottomLeft;
    [SerializeField] private Vector2 topRight;
    [Space(15)]

    [Header("Spawning Attempts")]
    [SerializeField] private int maxSpawnAttempts;
    [SerializeField] private Vector2[] worstCaseSpawnLocations;

    public List<GameObject> SpawnEnemies(List<EnemySpawningInfo> _enemies, int _numEnemies,
                                         Action<EnemySpawningInfo, int> _infoChooseAction = null,
                                         Action<GameObject> _spawnAction = null, Transform _parent = null)
    {
        Transform _player;
        if (!PlayerProvider.TryGetPlayer(out _player))
            return new();

        if (_enemies == null)
        {
            throw new ArgumentNullException(nameof(_enemies), "The enemies spawning info array should not be null.");
        }

        if (_enemies.Count == 0)
        {
            throw new ArgumentException("The enemies spawning info array should not have a length of 0.", nameof(_enemies));
        }

        // may move this into main for loop to speed up spawning if required
        #region Validation
        int _spawnChancesSum = 0;
        foreach (EnemySpawningInfo _enemy in _enemies)
        {
            if (_enemy.EnemyPrefab == null)
                throw new ArgumentNullException("One of the EnemySpawningInfo struct's EnemyPrefab " +
                    "in the given EnemySpawningInfo array was null.");

            if (_enemy.WeaponPrefab == null)
                throw new ArgumentNullException("One of the EnemySpawningInfo struct's WeaponPrefab " +
                    "in the given EnemySpawningInfo array was null.");

            if (!_enemy.WeaponPrefab.TryGetComponent(out Weapon _))
                throw new ArgumentException("One of the EnemySpawningInfo struct's WeaponPrefab " +
                    "did not have a Weapon component attached to it.");
            if (!_enemy.EnemyPrefab.TryGetComponent(out EnemyStateMachine _))
                throw new ArgumentException("One of the EnemySpawningInfo struct's EnemyPrefab " +
                    "did not have an EnemyStateMachine component attached to it, so it is likely not an enemy.");

            _spawnChancesSum += _enemy.SpawnChance;
        }

        if (_spawnChancesSum != 100)
        {
            throw new ArgumentException("The total spawn chances for the EnemySpawningInfo[] parameter should be 100, " +
                $"but it was {_spawnChancesSum}.");
        }
        #endregion

        List<GameObject> _spawnedEnemies = new();
        for (int i = 0; i < _numEnemies; i++)
        {
            Vector2 _spawnLocation = GetRandomSpawnPosition(_player);

            (EnemySpawningInfo, int) _spawningInfoAndIndex = GetSpawningInfoAndIndex(UnityEngine.Random.Range(0, 101), _enemies);

            _infoChooseAction?.Invoke(_spawningInfoAndIndex.Item1, _spawningInfoAndIndex.Item2);

            GameObject _enemy;
            Transform _enemyTrans;

            if (_parent == null)
            {
                 _enemy = Instantiate(_spawningInfoAndIndex.Item1.EnemyPrefab);
                _enemyTrans = _enemy.transform;
            }

            else
            {
                _enemy = Instantiate(_spawningInfoAndIndex.Item1.EnemyPrefab, _parent);
                _enemyTrans = _enemy.transform;
            }

            _enemyTrans.position = _spawnLocation;

            GameObject _weapon = Instantiate(_spawningInfoAndIndex.Item1.WeaponPrefab, _enemyTrans);
            _weapon.transform.rotation = Quaternion.identity;
            _weapon.SetActive(true);

            Weapon _weaponComponent = _weapon.GetComponent<Weapon>();
            _enemy.GetComponent<EnemyStateMachine>().Init(_weaponComponent);
            _enemy.GetComponent<EnemyWeaponManager>().Weapon = _weaponComponent;

            _spawnAction?.Invoke(_enemy);

            _spawnedEnemies.Add(_enemy);
        }

        return _spawnedEnemies;
    }

    public List<GameObject> SpawnEnemies(EnemySpawningInfo _enemyInfo, int _numEnemies, Action<GameObject> _spawnAction = null, Transform _parent = null)
    {
        if (!PlayerProvider.TryGetPlayer(out Transform _player))
            return new();

        #region Validation
        if (_enemyInfo.EnemyPrefab == null)
            throw new ArgumentNullException("The EnemySpawningInfo struct's EnemyPrefab is null.");

        if (_enemyInfo.WeaponPrefab == null)
            throw new ArgumentNullException("The EnemySpawningInfo struct's WeaponPrefab is null.");

        if (!_enemyInfo.WeaponPrefab.TryGetComponent(out Weapon _))
            throw new ArgumentException("The EnemySpawningInfo struct's WeaponPrefab did not have " +
                "a Weapon component attached to it.");
        if (!_enemyInfo.EnemyPrefab.TryGetComponent(out EnemyStateMachine _))
            throw new ArgumentException("The EnemySpawningInfo struct's EnemyPrefab did not have " +
                "an EnemyStateMachine component attached to it, so it is likely not an enemy.");

        if (_enemyInfo.SpawnChance != 100f)
            throw new ArgumentException("The total spawn chances for the EnemySpawningInfo[] parameter should be 100, " +
                $"but it was {_enemyInfo.SpawnChance}.");
        #endregion

        List<GameObject> _spawnedEnemies = new();
        for (int i = 0; i < _numEnemies; i++)
        {
            Vector2 _spawnLocation = GetRandomSpawnPosition(_player);

            GameObject _enemy;
            Transform _enemyTrans;

            if (_parent == null)
            {
                _enemy = Instantiate(_enemyInfo.EnemyPrefab);
                _enemyTrans = _enemy.transform;
            }

            else
            {
                _enemy = Instantiate(_enemyInfo.EnemyPrefab, _parent);
                _enemyTrans = _enemy.transform;
            }

            _enemyTrans.position = _spawnLocation;

            GameObject _weapon = Instantiate(_enemyInfo.WeaponPrefab, _enemyTrans);
            _weapon.transform.rotation = Quaternion.identity;
            _weapon.SetActive(true);

            Weapon _weaponComponent = _weapon.GetComponent<Weapon>();
            _enemy.GetComponent<EnemyStateMachine>().Init(_weaponComponent);
            _enemy.GetComponent<EnemyWeaponManager>().Weapon = _weaponComponent;

            _spawnAction?.Invoke(_enemy);

            _spawnedEnemies.Add(_enemy);
        }

        return _spawnedEnemies;
    }

    private Vector2 GetRandomSpawnPosition(Transform _player)
    {
        bool _initialSetting = Physics2D.queriesHitTriggers;
        Physics2D.queriesHitTriggers = false;

        Vector2 _spawnLocation = new(UnityEngine.Random.Range(bottomLeft.x, topRight.x), UnityEngine.Random.Range(bottomLeft.y, topRight.y));

        int _spawnAttempts = 1;
        while (_spawnAttempts < maxSpawnAttempts &&
               (Vector2.Distance(_player.position, _spawnLocation) < minDistanceFromPlayerToSpawn ||
                Physics2D.OverlapPoint(_spawnLocation) == null))
        {
            _spawnLocation = new(UnityEngine.Random.Range(bottomLeft.x, topRight.x), UnityEngine.Random.Range(bottomLeft.y, topRight.y));
            _spawnAttempts++;
        }

        // if too close to player despite trying other spawn locations
        if (Vector2.Distance(_player.position, _spawnLocation) < minDistanceFromPlayerToSpawn)
            _spawnLocation = worstCaseSpawnLocations[UnityEngine.Random.Range(0, worstCaseSpawnLocations.Length)];

        Physics2D.queriesHitTriggers = _initialSetting;

        return _spawnLocation;
    }

    private (EnemySpawningInfo, int) GetSpawningInfoAndIndex(int _spawnSeed, List<EnemySpawningInfo> _enemies)
    {
        int i = 0;
        int _min = 0;
        EnemySpawningInfo _enemySpawningInfo = _enemies[0];
        for (; i < _enemies.Count; i++)
        {
            if (_spawnSeed >= _min && _spawnSeed <= _min + _enemies[i].SpawnChance)
            {
                _enemySpawningInfo = _enemies[i];
                break;
            }

            _min += _enemies[i].SpawnChance;
        }

        return (_enemySpawningInfo, i);
    }
}
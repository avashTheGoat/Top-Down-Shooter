using UnityEngine;
using System;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [Header("Spawning Restrictions")]
    [Tooltip("The minimum distance from the player a point needs to be to be a valid spawn location")]
    [SerializeField] protected float minDistanceFromPlayerToSpawn;
    [SerializeField] protected Vector2 bottomLeft;
    [SerializeField] protected Vector2 topRight;
    [Space(15)]

    [Header("Spawning Attempts")]
    [SerializeField] protected int maxSpawnAttempts;
    [SerializeField] protected Vector2[] worstCaseSpawnLocations;

    public List<GameObject> SpawnObjects(List<SpawningInfo> _objects, int _numObjects,
                                         Action<SpawningInfo, int> _infoChooseAction = null,
                                         Action<GameObject> _spawnAction = null, Transform _parent = null)
    {
        Transform _player;
        if (!PlayerProvider.TryGetPlayer(out _player))
            return new();

        if (_objects == null)
        {
            throw new ArgumentNullException(nameof(_objects), "The enemies spawning info array should not be null.");
        }

        if (_objects.Count == 0)
        {
            throw new ArgumentException("The enemies spawning info array should not have a length of 0.", nameof(_objects));
        }

        // may move this into main for loop to speed up spawning if required
        #region Validation
        int _spawnChancesSum = 0;
        foreach (SpawningInfo _object in _objects)
        {
            if (_object.ObjectPrefab == null)
                throw new ArgumentNullException("One of the SpawningInfo struct's ObjectPrefab " +
                    "in the given SpawningInfo array was null.");

            _spawnChancesSum += _object.SpawnChance;
        }

        if (_spawnChancesSum != 100)
        {
            throw new ArgumentException("The total spawn chances for the SpawningInfo[] parameter should be 100, " +
                $"but it was {_spawnChancesSum}.");
        }
        #endregion

        List<GameObject> _spawnedObjects = new();
        for (int i = 0; i < _numObjects; i++)
        {
            Vector2 _spawnLocation = GetRandomSpawnPosition(_player);

            (SpawningInfo, int) _spawningInfoAndIndex = GetSpawningInfoAndIndex(UnityEngine.Random.Range(0, 101), _objects);

            _infoChooseAction?.Invoke(_spawningInfoAndIndex.Item1, _spawningInfoAndIndex.Item2);

            GameObject _object;

            if (_parent == null)
                _object = Instantiate(_spawningInfoAndIndex.Item1.ObjectPrefab);

            else
                _object = Instantiate(_spawningInfoAndIndex.Item1.ObjectPrefab, _parent);

            _object.GetComponent<Transform>().position = _spawnLocation;

            _spawnAction?.Invoke(_object);

            _spawnedObjects.Add(_object);
        }

        return _spawnedObjects;
    }

    public List<GameObject> SpawnObjects(SpawningInfo _objectInfo, int _numObjects, Action<GameObject> _spawnAction = null, Transform _parent = null)
    {
        if (!PlayerProvider.TryGetPlayer(out Transform _player))
            return new();

        #region Validation
        if (_objectInfo.ObjectPrefab == null)
            throw new ArgumentNullException("The SpawningInfo struct's ObjectPrefab is null.");

        if (!_objectInfo.ObjectPrefab.TryGetComponent(out EnemyStateMachine _))
            throw new ArgumentException("The SpawningInfo struct's ObjectPrefab did not have " +
                "an EnemyStateMachine component attached to it, so it is likely not an enemy.");

        if (_objectInfo.SpawnChance != 100f)
            throw new ArgumentException("The total spawn chances for the SpawningInfo[] parameter should be 100, " +
                $"but it was {_objectInfo.SpawnChance}.");
        #endregion

        List<GameObject> _spawnedObjects = new();
        for (int i = 0; i < _numObjects; i++)
        {
            Vector2 _spawnLocation = GetRandomSpawnPosition(_player);

            GameObject _object;

            if (_parent == null)
                _object = Instantiate(_objectInfo.ObjectPrefab);

            else
                _object = Instantiate(_objectInfo.ObjectPrefab, _parent);

            _object.GetComponent<Transform>().position = _spawnLocation;

            _spawnAction?.Invoke(_object);

            _spawnedObjects.Add(_object);
        }

        return _spawnedObjects;
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

    private (SpawningInfo, int) GetSpawningInfoAndIndex(int _spawnSeed, List<SpawningInfo> _enemies)
    {
        int i = 0;
        int _min = 0;
        SpawningInfo _SpawningInfo = _enemies[0];
        for (; i < _enemies.Count; i++)
        {
            if (_spawnSeed >= _min && _spawnSeed <= _min + _enemies[i].SpawnChance)
            {
                _SpawningInfo = _enemies[i];
                break;
            }

            _min += _enemies[i].SpawnChance;
        }

        return (_SpawningInfo, i);
    }
}
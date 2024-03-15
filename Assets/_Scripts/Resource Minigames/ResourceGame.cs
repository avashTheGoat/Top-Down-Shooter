using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class ResourceGame : MonoBehaviour
{
    public event Action<Dictionary<ResourceSO, int>> OnGameComplete;
    public event Action<string> OnUnableToStartGame;

    [Header("Resource Spawning")]
    [SerializeField] protected ResourceSourceInfo[] possibleResourceSources;
    [SerializeField] protected Vector2 lowerLeftSpawnBound;
    [SerializeField] protected Vector2 upperRightSpawnBound;
    [Space(15)]

    [SerializeField] protected Canvas gameUi;

    protected virtual void Awake()
    {
        int _spawnChanceSum = 0;
        foreach (var _resourceSource in possibleResourceSources)
            _spawnChanceSum += _resourceSource.SpawnChance;

        if (_spawnChanceSum != 100)
            throw new ArgumentException($"The sum of spawn chances cannot be non-hundred. It is {_spawnChanceSum}.");
    }

    public abstract void StartGame();

    protected ResourceSourceInfo GetRandomResourceSourceInfo()
    {
        int _spawnSeed = UnityEngine.Random.Range(1, 101);
        int _min = 0;
        ResourceSourceInfo _enemySpawningInfo = possibleResourceSources[0];
        for (int i = 0; i < possibleResourceSources.Length; i++)
        {
            if (_spawnSeed > _min && _spawnSeed < _min + possibleResourceSources[i].SpawnChance)
            {
                _enemySpawningInfo = possibleResourceSources[i];
                break;
            }

            _min += possibleResourceSources[i].SpawnChance;
        }

        return _enemySpawningInfo;
    }

    protected Vector2 GetRandomSpawnPos()
    {
        float _randX = UnityEngine.Random.Range(lowerLeftSpawnBound.x, upperRightSpawnBound.x);
        float _randY =UnityEngine.Random.Range(lowerLeftSpawnBound.y, upperRightSpawnBound.y);
        return new Vector2(_randX, _randY);
    }

    protected void InvokeOnGameComplete(Dictionary<ResourceSO, int> droppedResources) => OnGameComplete?.Invoke(droppedResources);
    protected void InvokeOnUnableToStartGame(string message) => OnUnableToStartGame?.Invoke(message);
}
using UnityEngine;
using System;

public abstract class ResourceGame : MonoBehaviour
{
    public event Action OnSuccessfulStart;
    public event Action<string> OnUnableToStartGame;
    public event Action<Inventory<Resource>> OnGameSuccessfullyComplete;
    public event Action<Inventory<Resource>, string> OnGameUnsuccessfullyComplete;

    [field: SerializeField] public Canvas GameUI { get; private set; }

    [Header("Resource Spawning")]
    [SerializeField] protected ResourceSourceInfoSO[] possibleResourceSources;

    protected virtual void Awake()
    {
        int _spawnChanceSum = 0;
        foreach (var _resourceSource in possibleResourceSources)
            _spawnChanceSum += _resourceSource.SpawnChance;

        if (_spawnChanceSum != 100)
            throw new ArgumentException($"The sum of spawn chances cannot be non-hundred. It is {_spawnChanceSum}.");
    }

    public abstract void StartGame();

    protected ResourceSourceInfoSO GetRandomResourceSourceInfo()
    {
        int _spawnSeed = UnityEngine.Random.Range(1, 101);
        int _min = 0;
        ResourceSourceInfoSO _enemySpawningInfo = possibleResourceSources[0];
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

    protected void InvokeOnSuccessfulStart() => OnSuccessfulStart?.Invoke();
    protected void InvokeOnUnableToStartGame(string _message) => OnUnableToStartGame?.Invoke(_message);
    protected void InvokeOnGameSuccessfullyComplete(Inventory<Resource> _droppedResources) => OnGameSuccessfullyComplete?.Invoke(_droppedResources);
    protected void InvokeOnGameUnsuccessfullyComplete(Inventory<Resource> _droppedResources, string _message) => OnGameUnsuccessfullyComplete?.Invoke(_droppedResources, _message);
}
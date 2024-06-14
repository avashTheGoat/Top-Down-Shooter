using UnityEngine;
using System.Collections.Generic;
using NavMeshPlus.Components;

public class MetalGame : ResourceGame, IProvider<List<GameObject>>
{
    public Timer GameTimer { get; private set; }

    [SerializeField] private bool[] isPrefabEnemy;
    [Space(15)]

    [Header("Dependencies")]
    [SerializeField] private NavMeshSurface surface;
    [SerializeField] private EnemySpawner enemySpawner;
    [Space(15)]

    [Header("Prefabs")]
    [SerializeField] private GameObject metalWeaponPrefab;
    [Space(15)]

    [Header("Setting")]
    [SerializeField] private GameObject mainSetting;
    [Space(15)]

    [Header("Spawning Min/Max")]
    [SerializeField] private int MinNumMetals;
    [SerializeField] private int MaxNumMetals;
    [Range(0f, 1f)] [SerializeField] private float MinPercentOfEnemyMetals;
    [Range(0f, 1f)] [SerializeField] private float MaxPercentOfEnemyMetals;
    [Space(15)]

    [Header("Spawning Location Restrictions")]
    [SerializeField] private float minMetalDistance;
    [SerializeField] private int maxSpawnTries;
    [SerializeField] private List<Vector2> worstCaseSpawnLocations;
    [Space(15)]

    [SerializeField] private float maxTime;

    private List<GameObject> spawnedMetals = new();
    private List<GameObject> spawnedMetalEnemies = new();
    private Inventory<Resource> droppedResources = new();

    private Vector2 preGamePlayerLocation;

    private ResourceSourceInfoSO[] enemyMetalInfos;
    private ResourceSourceInfoSO[] nonEnemyMetalInfos;

    protected override void Awake()
    {
        if (isPrefabEnemy.Length != possibleResourceSources.Length)
            Debug.LogError($"{nameof(isPrefabEnemy)} and {nameof(possibleResourceSources)} must be the same length");

        List<ResourceSourceInfoSO> _enemyMetalInfos = new();
        List<ResourceSourceInfoSO> _nonEnemyMetalInfos = new();
        for (int i = 0; i < isPrefabEnemy.Length; i++)
        {
            if (isPrefabEnemy[i])
                _enemyMetalInfos.Add(possibleResourceSources[i]);
            else _nonEnemyMetalInfos.Add(possibleResourceSources[i]);
        }

        enemyMetalInfos = _enemyMetalInfos.ToArray();
        nonEnemyMetalInfos = _nonEnemyMetalInfos.ToArray();

        base.Awake();
        GameTimer = new(maxTime);
    }

    private void Update()
    {
        // only happens when game ends and list gets reset to new
        if (spawnedMetals.Count == 0)
            return;

        if (IsGameOverSuccessfully())
        {
            if (PlayerProvider.TryGetPlayer(out Transform _player))
                _player.position = preGamePlayerLocation;

            InvokeOnGameSuccessfullyComplete(droppedResources);

            Game.SetActive(false);
            mainSetting.SetActive(true);
            
            GameTimer.Reset();
            spawnedMetals = new();
            spawnedMetalEnemies = new();
            droppedResources = new();

            return;
        }

        if (GameTimer.GetRemainingTime() == 0f)
        {
            if (PlayerProvider.TryGetPlayer(out Transform _player))
                _player.position = preGamePlayerLocation;

            InvokeOnGameUnsuccessfullyComplete(droppedResources, "You ran out of time!");

            Game.transform.DestroyChildren(_gameObject => spawnedMetals.Contains(_gameObject));
            Game.SetActive(false);
            mainSetting.SetActive(true);

            GameTimer.Reset();
            spawnedMetals = new();
            spawnedMetalEnemies = new();
            droppedResources = new();

            return;
        }

        GameTimer.Tick(Time.deltaTime);
    }

    public override void StartGame()
    {
        if (PlayerProvider.TryGetPlayer(out Transform _player))
        {
            InvokeOnSuccessfulStart();

            Game.SetActive(true);
            mainSetting.SetActive(false);

            preGamePlayerLocation = _player.position;

            int _numMetals = Random.Range(MinNumMetals, MaxNumMetals + 1);
            int _numEnemyMetals = (int)(Random.Range(MinPercentOfEnemyMetals, MaxPercentOfEnemyMetals) * _numMetals);

            for (int i = 0; i < _numEnemyMetals; i++)
            {
                ResourceSourceInfoSO _resourceSourceToSpawn = GetRandomResourceSourceInfo(enemyMetalInfos);
                GameObject _spawnedMetalEnemy = enemySpawner.SpawnEnemies
                                                (new EnemySpawningInfo(_resourceSourceToSpawn.ResourceObject, metalWeaponPrefab), 1)
                                                [0].gameObject;
                _spawnedMetalEnemy.GetComponent<ResourceSourceInfo>().ResourceSource = _resourceSourceToSpawn;

                spawnedMetals.Add(_spawnedMetalEnemy);
                spawnedMetalEnemies.Add(_spawnedMetalEnemy);

                IDamageable _damage = _spawnedMetalEnemy.GetComponent<IDamageable>();
                IKillable _kill = _spawnedMetalEnemy.GetComponent<IKillable>();

                _damage.OnDamage += DamageResource;
                _kill.OnKill += HarvestResource;
            }

            for (int i = 0; i < _numMetals - _numEnemyMetals; i++)
            {
                ResourceSourceInfoSO _resourceSourceToSpawn = GetRandomResourceSourceInfo(nonEnemyMetalInfos);
                // spawn enemy
                // add to spawnedMetals list
            }
        }
    }

    public List<GameObject> Provide() => spawnedMetalEnemies;

    #region Listeners
    private void HarvestResource(GameObject _metalEnemy)
    {
        ResourceSourceInfoSO _resourceSource = _metalEnemy.GetComponent<ResourceSourceInfo>().ResourceSource;

        int _numMetalsHarvested = Random.Range(_resourceSource.MinAmountDropped, _resourceSource.MaxAmountDropped + 1);
        droppedResources.Add(_resourceSource.ResourceDropped, _numMetalsHarvested);

        Destroy(_metalEnemy);
    }

    // spawn particles, etc.
    private void DamageResource(float _, GameObject __) { }
    #endregion

    private bool IsGameOverSuccessfully()
    {
        foreach (GameObject _spawnedMetal in spawnedMetals)
        {
            if (_spawnedMetal != null)
                return false;
        }

        return true;
    }
}
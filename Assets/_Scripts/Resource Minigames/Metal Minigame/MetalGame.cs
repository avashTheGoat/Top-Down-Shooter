using UnityEngine;
using NavMeshPlus.Components;
using System.Linq;
using System.Collections.Generic;

public class MetalGame : ResourceGame, IProvider<List<GameObject>>
{
    public Timer GameTimer { get; private set; }
    public Canvas GameUI => gameUI;

    [Header("UI")]
    [SerializeField] private Canvas gameUI;
    [Space(15)]

    [SerializeField] private bool[] isPrefabEnemy;
    [Space(15)]

    [Header("Dependencies")]
    [SerializeField] private NavMeshSurface surface;
    [SerializeField] private EnemySpawner enemyMetalSpawner;
    [SerializeField] private Spawner metalSpawner;
    [Space(15)]

    [Header("Prefabs")]
    [SerializeField] private GameObject enemyMetalWeaponPrefab;
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

    [SerializeField] private float maxTime;

    private List<GameObject> spawnedMetals = new();
    private List<GameObject> spawnedMetalEnemies = new();
    private Inventory<Resource> droppedResources = new();

    private Vector2 preGamePlayerLocation;

    List<ResourceSourceInfoSO> enemyMetalResourceInfos;
    List<ResourceSourceInfoSO> nonEnemyMetalResourceInfos;

    private List<EnemySpawningInfo> enemyMetalSpawningInfos;
    private List<SpawningInfo> nonEnemyMetalSpawningInfos;

    protected override void Awake()
    {
        if (isPrefabEnemy.Length != possibleResourceSources.Length)
            Debug.LogError($"{nameof(isPrefabEnemy)} and {nameof(possibleResourceSources)} must be the same length");

        enemyMetalResourceInfos = new();
        nonEnemyMetalResourceInfos = new();
        for (int i = 0; i < isPrefabEnemy.Length; i++)
        {
            if (isPrefabEnemy[i])
                enemyMetalResourceInfos.Add(possibleResourceSources[i]);
            else nonEnemyMetalResourceInfos.Add(possibleResourceSources[i]);
        }

        enemyMetalSpawningInfos = enemyMetalResourceInfos.Select
                                  (_enemyInfo => new EnemySpawningInfo(_enemyInfo.ResourceObject, enemyMetalWeaponPrefab, _spawnChance: _enemyInfo.SpawnChance))
                                  .ToList();

        nonEnemyMetalSpawningInfos = nonEnemyMetalResourceInfos.Select(_metalInfo => new SpawningInfo(_metalInfo.ResourceObject, _spawnChance: _metalInfo.SpawnChance))
                                     .ToList();

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
            ResetGame();

            return;
        }

        if (GameTimer.GetRemainingTime() == 0f)
        {
            if (PlayerProvider.TryGetPlayer(out Transform _player))
                _player.position = preGamePlayerLocation;

            InvokeOnGameUnsuccessfullyComplete(droppedResources, "You ran out of time!");

            Game.transform.DestroyChildren(_gameObject => spawnedMetals.Contains(_gameObject));
            ResetGame();

            return;
        }

        GameTimer.Tick(Time.deltaTime);
    }

    public override void StartGame()
    {
        if (!PlayerProvider.TryGetPlayer(out Transform _player))
            return;

        InvokeOnSuccessfulStart();

        Game.SetActive(true);
        gameUI.gameObject.SetActive(true);
        mainSetting.SetActive(false);

        preGamePlayerLocation = _player.position;

        int _numMetals = Random.Range(MinNumMetals, MaxNumMetals + 1);
        int _numEnemyMetals = (int)(Random.Range(MinPercentOfEnemyMetals, MaxPercentOfEnemyMetals) * _numMetals);

        ResourceSourceInfoSO _resourceSourceInfo = null;
        enemyMetalSpawner.SpawnEnemies(enemyMetalSpawningInfos, _numEnemyMetals,
                                        _infoChooseAction: (_info, _index) => _resourceSourceInfo = enemyMetalResourceInfos[_index],
                                        _spawnAction: _enemy =>
                                        {
                                            _enemy.GetComponent<ResourceSourceInfo>().ResourceSource = _resourceSourceInfo;

                                            spawnedMetals.Add(_enemy);
                                            spawnedMetalEnemies.Add(_enemy);

                                            IDamageable _damage = _enemy.GetComponent<IDamageable>();
                                            IKillable _kill = _enemy.GetComponent<IKillable>();

                                            _damage.OnDamage += DamageResourceEffects;
                                            _kill.OnKill += HarvestResource;
                                            _kill.OnKill += _deadEnemy => spawnedMetalEnemies.Remove(_deadEnemy); 
                                        }, _parent: Game.transform);

        metalSpawner.SpawnObjects(nonEnemyMetalSpawningInfos, _numMetals - _numEnemyMetals,
                                    _infoChooseAction: (_info, _index) => _resourceSourceInfo = nonEnemyMetalResourceInfos[_index],
                                    _spawnAction: _object =>
                                    {
                                        _object.GetComponent<ResourceSourceInfo>().ResourceSource = _resourceSourceInfo;
                                        spawnedMetals.Add(_object);

                                        IDamageable _damage = _object.GetComponent<IDamageable>();
                                        IKillable _kill = _object.GetComponent<IKillable>();

                                        _damage.OnDamage += DamageResourceEffects;
                                        _kill.OnKill += HarvestResource;
                                    }, _parent: Game.transform);
    }

    #region Listeners
    private void HarvestResource(GameObject _metalEnemy)
    {
        ResourceSourceInfoSO _resourceSource = _metalEnemy.GetComponent<ResourceSourceInfo>().ResourceSource;

        int _numMetalsHarvested = Random.Range(_resourceSource.MinAmountDropped, _resourceSource.MaxAmountDropped + 1);
        droppedResources.Add(_resourceSource.ResourceDropped, _numMetalsHarvested);

        Destroy(_metalEnemy);
    }

    // spawn particles, etc.
    private void DamageResourceEffects(float _, GameObject __) { }
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

    private void ResetGame()
    {
        Game.SetActive(false);
        mainSetting.SetActive(true);

        GameTimer.Reset();
        spawnedMetals = new();
        spawnedMetalEnemies = new();
        droppedResources = new();
    }

    List<GameObject> IProvider<List<GameObject>>.Provide() => spawnedMetalEnemies;
}
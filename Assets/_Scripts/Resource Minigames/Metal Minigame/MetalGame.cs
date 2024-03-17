using UnityEngine;
using System.Collections.Generic;

public class MetalGame : ResourceGame
{
    public Timer GameTimer { get; private set; }

    [SerializeField] private int MinNumMetals;
    [SerializeField] private int MaxNumMetals;

    [Header("Spawning Restrictions")]
    [SerializeField] private float minMetalDistance;
    [SerializeField] private int maxSpawnTries;
    [SerializeField] private List<Vector2> worstCaseSpawnLocations;
    [Space(15)]

    [SerializeField] private float maxTime;

    [SerializeField] private ParticleSystem harvestedMetalParticles;

    [SerializeField] private float pickaxeDamage;

    private ParticleSystem.MainModule harvestedMetalParticleSettings;

    private List<RectTransform> spawnedMetals = new();
    private Inventory droppedResources = new();

    protected override void Awake()
    {
        base.Awake();
        // harvestedMetalParticleSettings = harvestedMetalParticles.main;
        GameTimer = new(maxTime);
    }

    private void Update()
    {
        if (spawnedMetals.Count == 0)
            return;

        if (IsGameOver())
        {
            InvokeOnGameComplete(droppedResources.GetInventory());
            foreach (KeyValuePair<ResourceSO, int> _resourceCount in droppedResources.GetInventory())
            {
                print($"{_resourceCount.Key}: {_resourceCount.Value}");
            }
            return;
        }

        GameTimer.Tick(Time.deltaTime);
    }

    [ContextMenu("StartGame")]
    public override void StartGame()
    {
        if (PlayerProvider.TryGetPlayer(out Transform _player))
        {
            // if player doesn't have pickaxe, invoke OnUnableToPlay

            gameUi.enabled = true;

            int _numMetals = Random.Range(MinNumMetals, MaxNumMetals + 1);
            for (int i = 0; i < _numMetals; i++)
            {
                ResourceSourceInfo _resourceSourceToSpawn = GetRandomResourceSourceInfo();
                GameObject _spawnedObject = Instantiate(_resourceSourceToSpawn.ResourceObject, Vector2.zero, Quaternion.identity, gameUi.transform);

                RectTransform _trans = _spawnedObject.GetComponent<RectTransform>();
                _trans.localPosition = GetRandomValidSpawnPos();
                spawnedMetals.Add(_trans);

                ClickableResource _clickableResource = _spawnedObject.GetComponent<ClickableResource>();
                _clickableResource.SetMinResourceAmount(_resourceSourceToSpawn.MinAmount);
                _clickableResource.SetMaxResourceAmount(_resourceSourceToSpawn.MaxAmount);
                _clickableResource.Resource = _resourceSourceToSpawn.Resource;

                _clickableResource.OnClick += DamageResource;
                _clickableResource.OnKill += HarvestResource;
            }
        }
    }

    #region ClickableResource Listeners
    private void HarvestResource(GameObject _clickableResourceObject)
    {
        ClickableResource _clickableResource = _clickableResourceObject.GetComponent<ClickableResource>();

        int _numMetalsHarvested = Random.Range(_clickableResource.MinResourceAmount, _clickableResource.MaxResourceAmount + 1);
        droppedResources.Add(_clickableResource.Resource, _numMetalsHarvested);

/*        harvestedMetalParticleSettings.maxParticles = _numMetalsHarvested;

        harvestedMetalParticles.transform.position = clickableResource.transform.position;
        harvestedMetalParticles.Play();
*/
        Destroy(_clickableResource.gameObject);
    }

    private void DamageResource(ClickableResource _clickableResource) => _clickableResource.Damage(pickaxeDamage);
    #endregion

    #region Spawning Methods
    private Vector2 GetRandomValidSpawnPos()
    {
        int _numTries = 1;

        Vector2 _spawnPos = GetRandomSpawnPos();
        bool _isValid = IsPosValidSpawn(_spawnPos);
        while (!_isValid && _numTries <= maxSpawnTries)
        {
            _spawnPos = GetRandomSpawnPos();
            _isValid = IsPosValidSpawn(_spawnPos);

            _numTries++;
        }

        if (!_isValid)
            _spawnPos = worstCaseSpawnLocations[Random.Range(0, worstCaseSpawnLocations.Count)];

        return _spawnPos;
    }

    private bool IsPosValidSpawn(Vector2 _pos)
    {
        foreach (Transform _metal in spawnedMetals)
        {
            if (Vector2.Distance(_metal.localPosition, _pos) < minMetalDistance)
                return false;
        }

        return true;
    }
    #endregion

    private bool IsGameOver()
    {
        foreach (Transform _spawnedMetal in spawnedMetals)
        {
            if (_spawnedMetal != null)
                return false;
        }

        return true;
    }
}
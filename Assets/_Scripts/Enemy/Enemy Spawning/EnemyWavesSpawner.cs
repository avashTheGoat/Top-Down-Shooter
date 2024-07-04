using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class EnemyWavesSpawner : MonoBehaviour, IProvider<List<GameObject>>
{
    public event Action<int> OnWaveStart;
    public event Action<int> OnWaveComplete;
    public event Action OnAllWavesComplete;

    public event Action<GameObject> OnEnemySpawn;
    public event Action<GameObject, string> OnBossSpawn;

    public List<GameObject> SpawnedEnemies
    {
        get
        {
            spawnedEnemies.RemoveAll(_enemy =>  _enemy == null);
            return spawnedEnemies;
        }
    }

    public int TotalEnemies
    {
        get
        {
            if (isEndless)
                return int.MaxValue;

            return totalEnemies;
        }
    }

    public int TotalNumSpawnedEnemies { get; private set; }
    public int NumKilledEnemies { get; private set; }

    [SerializeField] private EnemySpawner spawner;

    private AnimationCurve numEnemiesAtWaveCount;
    private AnimationCurve secsToEnemiesAtWaveCount;
    private List<EnemySpawningInfos> spawnableEnemiesForEachWave;
    private bool isEndless = false;

    private int totalEnemies;
    private int wave = 1;
    private List<GameObject> spawnedEnemies = new();

    private float enemiesPerSecond;
    private float enemySpawnTimer = 0f;
    private int numSpawnedEnemies = 0;

    private bool haveAllWavesBeenCompleted = false;

    private List<EnemySpawningInfo> requiredEnemies = new();

    private void OnEnable()
    {
        OnWaveStart?.Invoke(1);
        UpdateRequiredEnemies(1);
    }

    private void Update()
    {
        if (AreWavesOver())
        {
            if (!haveAllWavesBeenCompleted)
                OnAllWavesComplete?.Invoke();

            haveAllWavesBeenCompleted = true;
            return;
        }

        enemySpawnTimer += Time.deltaTime;

        if (enemySpawnTimer * enemiesPerSecond >= 1 && numSpawnedEnemies < numEnemiesAtWaveCount.Evaluate(wave))
        {
            GameObject _spawnedEnemy = null;
            
            bool _isBoss = false;
            string _bossName = "";

            if (requiredEnemies.Count == 0)
            {
                int _curNightIndex = Mathf.Clamp(wave - 1, 0, spawnableEnemiesForEachWave.Count - 1);
                List<EnemySpawningInfo> _possibleEnemies = spawnableEnemiesForEachWave[_curNightIndex].SpawningInfos;

                _spawnedEnemy = spawner.SpawnEnemies(_possibleEnemies, 1, _infoChooseAction: (_chosen, _index) =>
                {
                    if (_chosen.IsOnlyEnemyOfType)
                        spawnableEnemiesForEachWave[wave - 1].SpawningInfos.Remove(_chosen);
                })[0];
            }

            else
            {
                int _chosenIndex = -1;
                _spawnedEnemy = spawner.SpawnEnemies(requiredEnemies, 1,
                    _infoChooseAction: (_chosen, _index) =>
                    {
                        _chosenIndex = _index;

                        if (_chosen.IsBoss)
                        {
                            _isBoss = _chosen.IsBoss;
                            _bossName = _chosen.BossName;
                        }

                        if (_chosen.IsOnlyEnemyOfType)
                        {
                            requiredEnemies.RemoveAt(_index);
                            spawnableEnemiesForEachWave[wave - 1].SpawningInfos.Remove(_chosen);

                            //UpdateSpawningInfoChances(spawnableEnemiesForEachWave[wave - 1]);
                        }
                    })[0];

                //requiredEnemies.RemoveAt(_chosenIndex);
            }

            _spawnedEnemy.GetComponent<IKillable>().OnKill += _ => NumKilledEnemies++;
            spawnedEnemies.Add(_spawnedEnemy);

            numSpawnedEnemies++;
            TotalNumSpawnedEnemies++;
            enemySpawnTimer = 0f;

            OnEnemySpawn?.Invoke(_spawnedEnemy.gameObject);

            if (_isBoss)
                OnBossSpawn?.Invoke(_spawnedEnemy.gameObject, _bossName);
        }

        if (SpawnedEnemies.Count == 0 && numSpawnedEnemies >= numEnemiesAtWaveCount.Evaluate(wave))
            NextWave();
    }

    public void SetWavesSettings(AnimationCurve _numEnemiesAtWaveCount, AnimationCurve _secsToSpawnEnemiesAtWaveCount,
                                 EnemySpawningInfos[] _spawnableEnemiesAtWaveCount, bool _isEndless)
    {
        numEnemiesAtWaveCount = _numEnemiesAtWaveCount;
        secsToEnemiesAtWaveCount = _secsToSpawnEnemiesAtWaveCount;
        isEndless = _isEndless;

        numEnemiesAtWaveCount.preWrapMode = WrapMode.Clamp;
        numEnemiesAtWaveCount.postWrapMode = WrapMode.Clamp;
        secsToEnemiesAtWaveCount.preWrapMode = WrapMode.Clamp;
        secsToEnemiesAtWaveCount.postWrapMode = WrapMode.Clamp;

        enemiesPerSecond = numEnemiesAtWaveCount.Evaluate(wave) / secsToEnemiesAtWaveCount.Evaluate(wave);

        spawnableEnemiesForEachWave = new();
        // copying so that enemy spawning infos can be removed
        foreach (EnemySpawningInfos _waveSpawnableEnemies in _spawnableEnemiesAtWaveCount)
        {
            EnemySpawningInfos _infosCopy = new();
            _infosCopy.SpawningInfos = new();

            spawnableEnemiesForEachWave.Add(_infosCopy);
            foreach (EnemySpawningInfo _enemySpawningInfo in _waveSpawnableEnemies.SpawningInfos)
                _infosCopy.SpawningInfos.Add(_enemySpawningInfo.Copy());
        }

        UpdateRequiredEnemies(1);

        if (isEndless)
            return;

        float _prevNumEnemies = -1;
        for (int wave = 1; wave <= spawnableEnemiesForEachWave.Count; wave++)
        {
            if (_prevNumEnemies == numEnemiesAtWaveCount.Evaluate(wave))
                break;

            _prevNumEnemies = numEnemiesAtWaveCount.Evaluate(wave);
            totalEnemies += Mathf.CeilToInt(_prevNumEnemies);
        }
    }

    public void ResetObj()
    {
        wave = 1;
        haveAllWavesBeenCompleted = false;

        enemySpawnTimer = 0f;
        numSpawnedEnemies = 0;
        enemiesPerSecond = 0f;

        TotalNumSpawnedEnemies = 0;
        NumKilledEnemies = 0;
        totalEnemies = 0;
        
        requiredEnemies = new();
        spawnedEnemies = new();
    }

    private void NextWave()
    {
        wave++;

        if (AreWavesOver())
            return;

        OnWaveComplete?.Invoke(wave - 1);

        enemySpawnTimer = 0f;
        numSpawnedEnemies = 0;
        enemiesPerSecond = numEnemiesAtWaveCount.Evaluate(wave) / secsToEnemiesAtWaveCount.Evaluate(wave);

        UpdateRequiredEnemies(wave);

        OnWaveStart?.Invoke(wave);
    }

    private bool AreWavesOver() => !isEndless && wave > spawnableEnemiesForEachWave.Count;

    private void UpdateRequiredEnemies(int _waveNum)
    {
        int _chanceSum = 0;
        foreach (EnemySpawningInfo _spawnableEnemy in spawnableEnemiesForEachWave[_waveNum - 1].SpawningInfos)
        {
            if (_spawnableEnemy.IsRequired)
            {
                requiredEnemies.Add(_spawnableEnemy);
                _chanceSum += _spawnableEnemy.SpawnChance;
            }
        }

        // shuffling the list
        System.Random _rng = new System.Random();
        requiredEnemies = requiredEnemies.OrderBy(_ => _rng.Next()).ToList();

        //UpdateSpawningInfoChances(requiredEnemies);
    }

    private void UpdateSpawningInfoChances(EnemySpawningInfos _spawningInfos)
    {
        int _oldSum = 0;
        foreach (EnemySpawningInfo _info in _spawningInfos.SpawningInfos)
            _oldSum += _info.SpawnChance;

        int _multi = 100 / (100 - _oldSum);
        foreach (EnemySpawningInfo _spawningInfo in _spawningInfos.SpawningInfos)
            _spawningInfo.SetSpawnChance(_spawningInfo.SpawnChance * _multi);

        int _newChanceSum = 0;
        foreach (EnemySpawningInfo _spawningInfo in _spawningInfos.SpawningInfos)
            _newChanceSum += _spawningInfo.SpawnChance;

        if (_newChanceSum == 100)
            return;

        if (_newChanceSum > 100)
        {
            List<EnemySpawningInfo> _leastLikelyToMost = _spawningInfos
                .SpawningInfos.OrderBy(_info => _info.SpawnChance).ToList();

            int _i = 0;
            while (_newChanceSum > 100)
            {
                if (_i >= _leastLikelyToMost.Count)
                    _i = 0;

                _leastLikelyToMost[_i].SetSpawnChance(_leastLikelyToMost[_i].SpawnChance - 1);
                _newChanceSum--;
            }
        }

        else
        {
            List<EnemySpawningInfo> _mostLikelyToLeast = _spawningInfos
                .SpawningInfos.OrderByDescending(_info => _info.SpawnChance).ToList();

            int _i = 0;
            while (_newChanceSum < 100)
            {
                if (_i >= _mostLikelyToLeast.Count)
                    _i = 0;

                _mostLikelyToLeast[_i].SetSpawnChance(_mostLikelyToLeast[_i].SpawnChance + 1);
                _newChanceSum++;
            }
        }
    }

    private void UpdateSpawningInfoChances(List<EnemySpawningInfo> _spawningInfos)
    {
        int _oldSum = 0;
        foreach (EnemySpawningInfo _info in _spawningInfos)
            _oldSum += _info.SpawnChance;

        float _multi = 100 / (float)(100 - _oldSum);
        for (int i = 0; i < _spawningInfos.Count; i++)
            _spawningInfos[i].SetSpawnChance((int)(_spawningInfos[i].SpawnChance * _multi));

        print("After mutlis, " + _spawningInfos.Stringify(_info => _info.SpawnChance.ToString()));

        int _newChanceSum = 0;
        foreach (EnemySpawningInfo _spawningInfo in _spawningInfos)
            _newChanceSum += _spawningInfo.SpawnChance;

        if (_newChanceSum == 100)
            return;

        if (_newChanceSum > 100)
        {
            List<EnemySpawningInfo> _leastLikelyToMost = _spawningInfos
                .OrderBy(_info => _info.SpawnChance).ToList();

            int _i = 0;
            while (_newChanceSum > 100)
            {
                if (_i >= _leastLikelyToMost.Count)
                    _i = 0;

                int _spawningInfosI = _spawningInfos.IndexOf(_leastLikelyToMost[_i]);
                _spawningInfos[_spawningInfosI].SetSpawnChance(_leastLikelyToMost[_i].SpawnChance - 1);
                _leastLikelyToMost[_spawningInfosI].SetSpawnChance(_leastLikelyToMost[_i].SpawnChance - 1);
                
                _newChanceSum--;
                _i++;
            }
        }

        else
        {
            List<EnemySpawningInfo> _mostLikelyToLeast = _spawningInfos
                .OrderByDescending(_info => _info.SpawnChance).ToList();

            int _i = 0;
            while (_newChanceSum < 100)
            {
                if (_i >= _mostLikelyToLeast.Count)
                    _i = 0;

                int _spawningInfosI = _spawningInfos.IndexOf(_mostLikelyToLeast[_i]);
                _spawningInfos[_spawningInfosI].SetSpawnChance(_mostLikelyToLeast[_i].SpawnChance - 1);
                _mostLikelyToLeast[_spawningInfosI].SetSpawnChance(_mostLikelyToLeast[_i].SpawnChance - 1);

                _newChanceSum++;
                _i++;
            }
        }

        print(_spawningInfos.Stringify(_info => _info.SpawnChance.ToString()));
    }

    public List<GameObject> Provide() => SpawnedEnemies;
}
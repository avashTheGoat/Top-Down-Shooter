using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyWavesSpawner : MonoBehaviour
{
    public event Action<int> OnWaveComplete;
    public event Action OnAllWavesComplete;
    public event Action OnEnemySpawn;

    public List<GameObject> SpawnedEnemies
    {
        get
        {
            spawnedEnemies.RemoveAll((_enemy) =>  _enemy == null);
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
    private EnemySpawningInfos[] spawnableEnemiesForEachWave;
    private bool isEndless = false;

    private int totalEnemies;
    private int wave = 1;
    private List<GameObject> spawnedEnemies = new();

    private float enemiesPerSecond;
    private float enemySpawnTimer = 0f;
    private int numSpawnedEnemies = 0;


    private bool haveAllWavesBeenCompleted = false;

    private void Update()
    {
        if (!isEndless && wave > spawnableEnemiesForEachWave.Length)
        {
            if (!haveAllWavesBeenCompleted) OnAllWavesComplete?.Invoke();

            haveAllWavesBeenCompleted = true;
            return;
        }

        enemySpawnTimer += Time.deltaTime;

        if (enemySpawnTimer * enemiesPerSecond >= 1 && numSpawnedEnemies < numEnemiesAtWaveCount.Evaluate(wave))
        {
            int _possibleEnemiesIndex = Mathf.Clamp(wave - 1, 0, spawnableEnemiesForEachWave.Length - 1);
            EnemySpawningInfo[] _possibleEnemies = spawnableEnemiesForEachWave[_possibleEnemiesIndex].enemySpawningInfos;

            List<GameObject> _newSpawnedEnemies = spawner.SpawnEnemies(_possibleEnemies, 1);
            foreach (GameObject _spawnedEnemy in _newSpawnedEnemies)
                _spawnedEnemy.GetComponent<IKillable>().OnKill += _ => NumKilledEnemies++;

            spawnedEnemies.AddRange(_newSpawnedEnemies);
            
            numSpawnedEnemies++;
            TotalNumSpawnedEnemies++;
            enemySpawnTimer = 0f;

            OnEnemySpawn?.Invoke();
        }

        if (SpawnedEnemies.Count == 0 && numSpawnedEnemies >= numEnemiesAtWaveCount.Evaluate(wave))
            NextWave();
        print("Spawned enemies count: " + SpawnedEnemies.Count);
    }

    private void NextWave()
    {
        Debug.LogWarning("Advancing to next wave");

        OnWaveComplete?.Invoke(wave);

        wave++;
        enemySpawnTimer = 0f;
        numSpawnedEnemies = 0;

        enemiesPerSecond = numEnemiesAtWaveCount.Evaluate(wave) / secsToEnemiesAtWaveCount.Evaluate(wave);
    }

    public void SetWavesSettings(AnimationCurve _numEnemiesAtWaveCount, AnimationCurve _secsToSpawnEnemiesAtWaveCount,
        EnemySpawningInfos[] _spawnableEnemiesAtWaveCount, bool _isEndless)
    {
        numEnemiesAtWaveCount = _numEnemiesAtWaveCount;
        secsToEnemiesAtWaveCount = _secsToSpawnEnemiesAtWaveCount;
        spawnableEnemiesForEachWave = _spawnableEnemiesAtWaveCount;
        isEndless = _isEndless;

        numEnemiesAtWaveCount.preWrapMode = WrapMode.Clamp;
        numEnemiesAtWaveCount.postWrapMode = WrapMode.Clamp;
        secsToEnemiesAtWaveCount.preWrapMode = WrapMode.Clamp;
        secsToEnemiesAtWaveCount.postWrapMode = WrapMode.Clamp;

        enemiesPerSecond = numEnemiesAtWaveCount.Evaluate(wave) / secsToEnemiesAtWaveCount.Evaluate(wave);

        if (isEndless)
            return;

        float _prevNumEnemies = -1;
        for (int wave = 1; wave <= spawnableEnemiesForEachWave.Length; wave++)
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
        enemySpawnTimer = 0f;
        numSpawnedEnemies = 0;
        TotalNumSpawnedEnemies = 0;
        NumKilledEnemies = 0;
        totalEnemies = 0;
    }
}
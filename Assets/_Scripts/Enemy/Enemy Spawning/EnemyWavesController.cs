using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyWavesController : MonoBehaviour
{
    public event Action<int> OnWaveComplete;
    public event Action OnAllWavesComplete;
    public event Action OnEnemySpawn;

    public int MAX_WAVE_COUNT;

    public List<GameObject> SpawnedEnemies
    {
        get
        {
            spawnedEnemies.RemoveAll((_enemy) => { return _enemy == null; });
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

    [SerializeField] private EnemySpawner spawner;

    [Header("Waves Info")]
    [SerializeField] private AnimationCurve numEnemiesAtWaveCount;
    [SerializeField] private AnimationCurve timeToSpawnAllEnemiesAtWaveCount;
    [SerializeField] private EnemySpawningInfos[] spawnableEnemiesForEachWave;
    [Space(15)]

    [SerializeField] private bool isEndless = false;

    private int totalEnemies;

    private int wave = 1;
    private List<GameObject> spawnedEnemies = new();

    private float enemiesPerSecond;
    private float enemySpawnTimer = 0f;
    private int numSpawnedEnemies = 0;


    private bool haveAllWavesBeenCompleted = false;

    private void Awake()
    {
        numEnemiesAtWaveCount.preWrapMode = WrapMode.Clamp;
        numEnemiesAtWaveCount.postWrapMode = WrapMode.Clamp;
        timeToSpawnAllEnemiesAtWaveCount.preWrapMode = WrapMode.Clamp;
        timeToSpawnAllEnemiesAtWaveCount.postWrapMode = WrapMode.Clamp;

        enemiesPerSecond = numEnemiesAtWaveCount.Evaluate(wave) / timeToSpawnAllEnemiesAtWaveCount.Evaluate(wave);

        if (isEndless)
            return;

        float _prevNumEnemies = -1;
        for (int wave = 0; wave <= MAX_WAVE_COUNT; wave++)
        {
            if (_prevNumEnemies == numEnemiesAtWaveCount.Evaluate(wave))
                break;

            _prevNumEnemies = numEnemiesAtWaveCount.Evaluate(wave);
            totalEnemies += Mathf.CeilToInt(_prevNumEnemies);
        }
    }

    private void Update()
    {
        if (!isEndless && wave >= spawnableEnemiesForEachWave.Length)
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
            spawnedEnemies.AddRange(spawner.SpawnEnemies(_possibleEnemies, 1));
            
            numSpawnedEnemies++;
            TotalNumSpawnedEnemies++;
            enemySpawnTimer = 0f;

            OnEnemySpawn?.Invoke();
        }

        if (SpawnedEnemies.Count == 0 && numSpawnedEnemies >= numEnemiesAtWaveCount.Evaluate(wave))
        {
            NextWave();
        }
    }

    private void NextWave()
    {
        OnWaveComplete?.Invoke(wave);

        wave++;
        enemySpawnTimer = 0f;
        numSpawnedEnemies = 0;

        enemiesPerSecond = numEnemiesAtWaveCount.Evaluate(wave) / timeToSpawnAllEnemiesAtWaveCount.Evaluate(wave);
    }
}
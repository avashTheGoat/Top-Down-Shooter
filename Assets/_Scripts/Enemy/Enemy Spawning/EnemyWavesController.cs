using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyWavesController : MonoBehaviour
{
    public event Action<int> OnWaveComplete;
    public event Action OnAllWavesComplete;
    public event Action OnEnemySpawn;

    public List<GameObject> SpawnedEnemies
    {
        get
        {
            spawnedEnemies.RemoveAll((_enemy) => { return _enemy == null; });
            return spawnedEnemies;
        }
    }

    [SerializeField] private EnemySpawner spawner;

    [Header("Waves Info")]
    [SerializeField] private AnimationCurve numEnemiesAtWaveCount;
    [SerializeField] private AnimationCurve timeToSpawnAllEnemiesAtWaveCount;
    [SerializeField] private EnemySpawningInfos[] spawnableEnemiesForEachWave;
    [Space(15)]

    [SerializeField] private bool isEndless = false;

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
            enemySpawnTimer = 0f;
            spawnedEnemies.AddRange(spawner.SpawnEnemies(spawnableEnemiesForEachWave[Math.Clamp(wave - 1, 0, spawnableEnemiesForEachWave.Length - 1)].enemySpawningInfos, 1));
            numSpawnedEnemies++;

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
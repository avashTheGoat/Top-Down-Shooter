using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyWavesController : MonoBehaviour
{
    public event Action<int> OnWaveComplete;
    public event Action OnAllWavesComplete;
    public event Action OnEnemySpawn;

    [field: SerializeField] public bool IsEndless { get; private set; } = false;

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
            if (IsEndless)
                return int.MaxValue;

            return totalEnemies;
        }
    }

    public int TotalNumSpawnedEnemies { get; private set; }
    public int NumKilledEnemies { get; private set; }

    [SerializeField] private EnemySpawner spawner;

    [Header("Waves Info")]
    [Tooltip("Animation curve that specifies the number of enemies that will spawn (y) on a certain wave (x)."
    + "AT EVERY INTEGER WAVE OF THE CURVE, THE Y-VALUE ROUNDED UP MUST BE DIFFERENT TO THE PREVIOUS INTEGER " +
    "WAVE Y-VALUE ROUNDED UP")]
    [SerializeField] private AnimationCurve numEnemiesAtWaveCount;
    [SerializeField] private AnimationCurve timeToSpawnAllEnemiesAtWaveCount;
    [SerializeField] private EnemySpawningInfos[] spawnableEnemiesForEachWave;
    [Space(15)]

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

        if (IsEndless)
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

    private void Update()
    {
        if (!IsEndless && wave >= spawnableEnemiesForEachWave.Length)
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
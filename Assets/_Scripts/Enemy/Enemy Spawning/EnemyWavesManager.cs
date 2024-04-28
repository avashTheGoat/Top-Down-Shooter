using UnityEngine;

public class EnemyWavesManager : MonoBehaviour
{
    public int NightNum { get; private set; } = 0;

    [Header("Dependencies")]
    [SerializeField] private DayNightManager dayNightManager;
    [SerializeField] private EnemyWavesSpawner wavesSpawner;
    [Space(15)]

    [Header("Settings")]
    [SerializeField] private EntireNightEnemySpawningInfos[] enemies;
    [Min(0)]
    [SerializeField] private int startingNightNum = 0;

    private void Awake() => NightNum = startingNightNum;

    private void Start()
    {
        dayNightManager.OnDayEnd += () =>
        {
            EntireNightEnemySpawningInfos _curNightInfo = enemies[Mathf.Clamp(NightNum, 0, enemies.Length - 1)];
            wavesSpawner.SetWavesSettings
            (
                _curNightInfo.numEnemiesAtWaveCount, _curNightInfo.secsToSpawnEnemiesAtWaveCount,
                _curNightInfo.spawnableEnemiesAtWaveCount, _curNightInfo.IsEndless
            );

            wavesSpawner.enabled = true;

            NightNum++;
        };

        dayNightManager.OnNightEnd += () =>
        {
            wavesSpawner.enabled = false;
            wavesSpawner.ResetObj();
        };
    }
}
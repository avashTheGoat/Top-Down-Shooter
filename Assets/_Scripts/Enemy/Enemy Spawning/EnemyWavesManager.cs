using UnityEngine;

public class EnemyWavesManager : MonoBehaviour
{
    [SerializeField] private DayNightManager dayNightManager;
    [SerializeField] private EnemyWavesSpawner wavesSpawner;

    [SerializeField] private EntireNightEnemySpawningInfos[] enemies;

    private int nightCounter = 0;

    private void Start()
    {
        dayNightManager.OnDayEnd += () =>
        {
            EntireNightEnemySpawningInfos _curNightInfo = enemies[Mathf.Clamp(nightCounter, 0, enemies.Length - 1)];
            wavesSpawner.SetWavesSettings
            (
                _curNightInfo.numEnemiesAtWaveCount, _curNightInfo.secsToSpawnEnemiesAtWaveCount,
                _curNightInfo.spawnableEnemiesAtWaveCount, _curNightInfo.IsEndless
            );

            wavesSpawner.enabled = true;

            nightCounter++;
        };

        dayNightManager.OnNightEnd += () =>
        {
            wavesSpawner.enabled = false;
            wavesSpawner.ResetObj();
        };
    }
}
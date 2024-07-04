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

    private void Awake()
    {
        #region Spawning Info Checking
        for (int i = 0; i < enemies.Length; i++)
        {
            EntireNightEnemySpawningInfos _nightSpawningInfo = enemies[i];

            if (_nightSpawningInfo.IsEndless && i != enemies.Length - 1)
                Debug.LogWarning("There is a night spawning info that is endless but isn't the last one, " +
                    "so there are some nights that will not play.");

            foreach (EnemySpawningInfos _waveEnemies in _nightSpawningInfo.SpawnableEnemiesAtWave)
            {
                foreach (EnemySpawningInfo _enemy in _waveEnemies.SpawningInfos)
                {
                    if (_enemy.IsBoss && !_enemy.IsRequired)
                        Debug.LogError("All bosses should have the Is Required box checked.");
                }
            }
        }
        #endregion

        NightNum = startingNightNum;
    }

    private void Start()
    {
        dayNightManager.OnDayEnd += () =>
        {
            EntireNightEnemySpawningInfos _curNightInfo = enemies[Mathf.Clamp(NightNum, 0, enemies.Length - 1)];
            
            wavesSpawner.ResetObj();
            wavesSpawner.SetWavesSettings
            (
                _curNightInfo.NumEnemiesAtWave, _curNightInfo.SecsToSpawnEnemiesAtWave,
                _curNightInfo.SpawnableEnemiesAtWave, _curNightInfo.IsEndless
            );
            wavesSpawner.enabled = true;

            NightNum++;
        };

        dayNightManager.OnNightEnd += () => wavesSpawner.enabled = false;
    }
}
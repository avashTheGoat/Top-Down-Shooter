using UnityEngine;
using TMPro;

public class EnemyWavesReceiver : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private EnemyWavesSpawner enemyWaves;
    [SerializeField] private EnemyWavesManager enemyWavesManager;
    [SerializeField] private DayCycleReceiver dayCycleReceiver;
    [Space(15)]

    [Header("UI Dependencies")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI nightSurviveText;
    [Space(15)]

    private Fadeable waveTextFadeable;
    private Fadeable nightSurviveFadeable;

    private void Start()
    {
        waveTextFadeable = waveText.GetComponent<Fadeable>();
        nightSurviveFadeable = nightSurviveText.GetComponent<Fadeable>();

        enemyWaves.OnWaveStart += _waveNum =>
        {
            waveText.text = $"Wave {_waveNum}";

            if (_waveNum == 1)
                waveTextFadeable.FadeInOut(dayCycleReceiver.GetDayTimerFadeOutTime());
            else waveTextFadeable.FadeInOut(0);
        };

        enemyWaves.OnAllWavesComplete += () =>
        {
            nightSurviveText.GetComponent<TextMeshProUGUI>().text = $"Night {enemyWavesManager.NightNum} survived";
            nightSurviveFadeable.FadeInOut(0);
        };
    }

    public float GetNightSurviveTotalVisibleTime() => nightSurviveFadeable.FadeInSecsInOut + nightSurviveFadeable.FadeOutSecsInOut + nightSurviveFadeable.VisibleSecs;
}
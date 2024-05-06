using UnityEngine;
using TMPro;

public class EnemyWavesReciever : MonoBehaviour
{
    [SerializeField] private EnemyWavesSpawner enemyWaves;
    [SerializeField] private EnemyWavesManager enemyWavesManager;

    [Header("Wave Complete")]
    [SerializeField] private CanvasGroup waveText;
    [SerializeField] private float fadeInDurationWaveComplete;
    [SerializeField] private float fullyVisibleDurationWaveComplete;
    [SerializeField] private float fadeOutDurationWaveComplete;

    [Header("All Waves Complete")]
    [SerializeField] private CanvasGroup nightSurviveText;
    [SerializeField] private float fadeInDurationAllWavesComplete;
    [SerializeField] private float fullyVisibleDurationAllWavesComplete;
    [SerializeField] private float fadeOutDurationAllWavesComplete;

    private UIEffects uiEffects;

    private void Awake() => uiEffects = new(this);

    private void Start()
    {
        enemyWaves.OnWaveStart += _waveNum =>
        {
            waveText.GetComponent<TextMeshProUGUI>().text = $"Wave {_waveNum}";
            uiEffects.FadeInAndOut(waveText, fadeInDurationWaveComplete,
            fullyVisibleDurationWaveComplete, fadeOutDurationWaveComplete);
        };

        enemyWaves.OnAllWavesComplete += () => 
        {
            nightSurviveText.GetComponent<TextMeshProUGUI>().text = $"Night {enemyWavesManager.NightNum} survived";
            uiEffects.FadeInAndOut(nightSurviveText, fadeInDurationAllWavesComplete,
            fullyVisibleDurationAllWavesComplete, fadeOutDurationAllWavesComplete);
        };
    }
}
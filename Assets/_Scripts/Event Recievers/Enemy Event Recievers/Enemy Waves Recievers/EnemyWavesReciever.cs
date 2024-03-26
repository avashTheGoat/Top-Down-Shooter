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

    private void Start()
    {
        enemyWaves.OnWaveStart += _waveNum =>
        {
            waveText.GetComponent<TextMeshProUGUI>().text = $"Wave {_waveNum}";
            UIEffectsManager.Instance.FadeInAndOut(waveText, fadeInDurationWaveComplete,
            fullyVisibleDurationWaveComplete, fadeOutDurationWaveComplete);
        };

        enemyWaves.OnAllWavesComplete += () => 
        {
            nightSurviveText.GetComponent<TextMeshProUGUI>().text = $"Night {enemyWavesManager.NightNum} survived";
            UIEffectsManager.Instance.FadeInAndOut(nightSurviveText, fadeInDurationAllWavesComplete,
            fullyVisibleDurationAllWavesComplete, fadeOutDurationAllWavesComplete);
        };
    }
}
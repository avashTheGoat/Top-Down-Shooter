using UnityEngine;
using System.Collections.Generic;

public class DayCycleReceiver : MonoBehaviour
{
    [Header("Dependenices")]
    [SerializeField] private DayNightManager dayNightManager;
    [SerializeField] private EnemyWavesReceiver enemyWaves;
    [Space(15)]

    [Header("UI Dependencies (DO NOT INCLUDE THESE IN LISTS)")]
    [SerializeField] private DayTimeTimerUI dayTimeTimerUI;
    [Space(15)]
    
    [Header("Day Start Fade")]
    [SerializeField] private List<Fadeable> dayStartFadeIn;
    [SerializeField] private List<Fadeable> dayStartFadeOut;
    [Space(15)]

    [Header("Day End Fade")]
    [SerializeField] private List<Fadeable> dayEndFadeIn;
    [SerializeField] private List<Fadeable> dayEndFadeOut;

    private void Awake()
    {
        dayNightManager.OnNightEnd += () =>
        {
            dayTimeTimerUI.FadeIn(enemyWaves.GetNightSurviveTotalVisibleTime());

            dayStartFadeIn.ForEach(_fadeable => _fadeable.FadeIn());
            dayStartFadeOut.ForEach(_fadeable => _fadeable.FadeOut());
        };

        dayNightManager.OnDayEnd += () =>
        {
            dayTimeTimerUI.FadeOut();

            dayEndFadeIn.ForEach(_fadeable => _fadeable.FadeIn());
            dayEndFadeOut.ForEach(_fadeable => _fadeable.FadeOut());
        };
    }
}
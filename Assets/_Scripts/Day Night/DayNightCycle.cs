using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

[ExecuteAlways]
public class DayNightCycle : MonoBehaviour
{
    [Tooltip("A float from 0 to 1 representing the progress in the day.")]
    [field: SerializeField] public float DayPercentProgress { get; private set; }

    [SerializeField] private Light2D globalLight;

    [Header("Day-Night Cycle Settings")]
    [SerializeField] private Gradient ambienceGradient;

    private void Update() => globalLight.color = ambienceGradient.Evaluate(DayPercentProgress);

    public void SetDayPercentProgress(float _newProgress)
    {
        if (_newProgress < 0 || _newProgress > 1)
            throw new ArgumentException($"_newProgress cannot be less than 0 or greater than 1. It is {_newProgress}");

        DayPercentProgress = _newProgress;
    }
}
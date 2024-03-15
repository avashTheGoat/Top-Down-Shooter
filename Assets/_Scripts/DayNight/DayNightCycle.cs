using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class DayNightCycle : MonoBehaviour
{
    [Range(0f, 1f)]
    public float DayPercentProgress;

    [SerializeField] private Light2D globalLight;

    [Header("Day-Night Cycle Settings")]
    [SerializeField] private Gradient ambienceGradient;

    private void Update() => globalLight.color = ambienceGradient.Evaluate(DayPercentProgress);
}
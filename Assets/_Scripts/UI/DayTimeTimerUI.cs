using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI), typeof(CanvasGroup))]
public class DayTimeTimerUI : Fadeable
{
    [Header("Dependencies")]
    [SerializeField] private DayNightManager dayNightManager;
    [Space(15)]

    private TextMeshProUGUI text;

    protected override void Awake()
    {
        base.Awake();

        text = GetComponent<TextMeshProUGUI>();
        canvasGroup.alpha = 1;
    }

    private void Start() => dayNightManager.SubscribeToDayTimerOnTick(_timer => text.text = ((int)_timer.GetRemainingTime()).ToString());
}
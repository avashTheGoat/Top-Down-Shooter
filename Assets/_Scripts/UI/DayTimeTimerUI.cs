using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DayTimeTimerUI : MonoBehaviour
{
    [SerializeField] private DayNightManager dayNightManager;

    private TextMeshProUGUI text;

    private void Awake() => text = GetComponent<TextMeshProUGUI>();

    private void Start()
    {
        dayNightManager.SubscribeToDayTimerOnTick(_timer => text.text = ((int)_timer.GetRemainingTime()).ToString());
        dayNightManager.OnDayEnd += () => text.enabled = false;
        dayNightManager.OnNightEnd += () => text.enabled = true;
    }
}
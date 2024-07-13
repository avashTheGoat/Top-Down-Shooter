using UnityEngine;

public class DaySkipReceiver : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private SkipButtonUI skipButton;
    [SerializeField] private DayNightManager dayNightManager;
    [SerializeField] private PlayerHealth playerHealth;
    [Space(15)]

    [Header("Settings")]
    [SerializeField] [Range(0f, 1f)] private float maxPlayerHealPercent;

    private void Start()
    {
        skipButton.OnSkip += () =>
        {
            playerHealth.Heal(maxPlayerHealPercent * playerHealth.GetMaxHealth() * dayNightManager.GetRemainingDayTime() / dayNightManager.DayTimeSecondsLength);

            dayNightManager.SkipToNight();
            skipButton.Button.interactable = false;
        };

        dayNightManager.OnNightEnd += () => skipButton.Button.interactable = true;
    }
}
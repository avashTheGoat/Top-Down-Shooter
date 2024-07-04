using UnityEngine;

public class DaySkipReceiver : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private SkipButtonUI skipButton;
    [SerializeField] private DayNightManager dayNightManager;

    private void Start()
    {
        skipButton.OnSkip += () =>
        {
            dayNightManager.SkipToNight();
            skipButton.Button.interactable = false;
        };

        dayNightManager.OnNightEnd += () => skipButton.Button.interactable = true;
    }
}
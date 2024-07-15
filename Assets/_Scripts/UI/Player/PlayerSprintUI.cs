using UnityEngine;
using UnityEngine.UI;

public class PlayerSprintUI : Fadeable
{
    [Header("Sprint Bar UI")]
    [SerializeField] private PlayerSprintStamina playerSprint;
    [SerializeField] private Image sprintBar;
    [Space(15)]

    [SerializeField] private float timeForSprintToFadeOut;

    private CanvasGroup sprintBarParentCanvasGroup;

    private bool isFadingIn = false;
    private Timer sprintBarFadeOutTimer;

    protected override void Awake()
    {
        base.Awake();

        sprintBarParentCanvasGroup = GetComponent<CanvasGroup>();
        sprintBarParentCanvasGroup.alpha = 0f;

        sprintBarFadeOutTimer = new(timeForSprintToFadeOut);
        sprintBarFadeOutTimer.OnComplete += () =>
        {
            FadeOut();
            isFadingIn = false;
            sprintBarFadeOutTimer.Reset();
        };

        sprintBar.fillAmount = Mathf.Clamp(playerSprint.Stamina / playerSprint.MaxStamina, 0f, 1f);
    }

    private void Start()
    {
        playerSprint.OnSprint += () =>
        {
            if (!isFadingIn)
            {
                FadeIn();
                sprintBarFadeOutTimer.Reset();
                isFadingIn = true;
            }

            sprintBar.fillAmount = Mathf.Clamp(playerSprint.Stamina / playerSprint.MaxStamina, 0f, 1f);
        };
    }

    private void Update() => sprintBarFadeOutTimer.Tick(Time.deltaTime);
}
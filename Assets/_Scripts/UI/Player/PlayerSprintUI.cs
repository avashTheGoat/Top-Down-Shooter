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
    private GameObject sprintBarParent;

    private float prevStamina;
    private Timer sprintFadeOutTimer;

    protected override void Awake()
    {
        base.Awake();

        sprintBarParent = sprintBar.transform.parent.parent.gameObject;
        sprintBarParentCanvasGroup = sprintBarParent.GetComponent<CanvasGroup>();
        sprintBarParentCanvasGroup.alpha = 0f;

        sprintFadeOutTimer = new(timeForSprintToFadeOut);
        sprintFadeOutTimer.OnComplete += () => FadeOut();
    }

    private void Start()
    {
        sprintBar.fillAmount = Mathf.Clamp(playerSprint.Stamina / playerSprint.MaxStamina, 0f, 1f);
        prevStamina = playerSprint.Stamina;
    }

    private void Update()
    {
        if (playerSprint.Stamina < prevStamina)
        {
            if (!IsSprintBarVisible())
            {
                FadeIn();
                sprintFadeOutTimer.Reset();
            }
        }

        else
            sprintFadeOutTimer.Tick(Time.deltaTime);

        sprintBar.fillAmount = Mathf.Clamp(playerSprint.Stamina / playerSprint.MaxStamina, 0f, 1f);
        prevStamina = playerSprint.Stamina;
    }

    private bool IsSprintBarVisible() => sprintBarParentCanvasGroup.alpha != 0f;
}
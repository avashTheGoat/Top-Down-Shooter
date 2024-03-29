using UnityEngine;
using UnityEngine.UI;

public class PlayerSprintUI : MonoBehaviour
{
    [SerializeField] private PlayerSprintStamina playerSprint;
    [SerializeField] private Image sprintBar;

    [Header("Fading")]
    [Min(0)]
    [SerializeField] private float fadeInTime;
    [Min(0)]
    [SerializeField] private float fadeOutTime;
    [Min(0)]
    [SerializeField] private float timeForSprintToFadeOut;

    private CanvasGroup sprintBarParentCanvasGroup;
    private GameObject sprintBarParent;

    private float prevStamina;
    private Timer sprintFadeOutTimer;

    private void Awake()
    {
        sprintBarParent = sprintBar.transform.parent.parent.gameObject;
        sprintBarParentCanvasGroup = sprintBarParent.GetComponent<CanvasGroup>();
        sprintBarParentCanvasGroup.alpha = 0f;

        sprintFadeOutTimer = new(timeForSprintToFadeOut);
        sprintFadeOutTimer.OnComplete += () => UIEffectsManager.Instance.FadeOut(sprintBarParentCanvasGroup, fadeOutTime);
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
                UIEffectsManager.Instance.FadeIn(sprintBarParentCanvasGroup, fadeInTime);

            sprintFadeOutTimer.Reset();
        }

        else
        {
            if (sprintBarParent.activeInHierarchy)
                sprintFadeOutTimer.Tick(Time.deltaTime);
        }

        sprintBar.fillAmount = Mathf.Clamp(playerSprint.Stamina / playerSprint.MaxStamina, 0f, 1f);
        prevStamina = playerSprint.Stamina;
    }

    private bool IsSprintBarVisible() => sprintBarParentCanvasGroup.alpha != 0f;
}
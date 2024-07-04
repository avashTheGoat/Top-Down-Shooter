using UnityEngine;
using System;

[RequireComponent(typeof(CanvasGroup))]
public class Fadeable : MonoBehaviour
{
    public event Action OnFadeComplete;

    public float FadeInSecs => fadeInSecs;
    public float FadeOutSecs => fadeOutSecs;

    public float FadeInSecsInOut => fadeInSecsInOut;
    public float VisibleSecs => visibleSecs;
    public float FadeOutSecsInOut => fadeOutSecsInOut;

    [Header("Fade In")]
    [SerializeField] protected float fadeInSecs;
    [Space(15)]

    [Header("Fade Out")]
    [SerializeField] protected float fadeOutSecs;
    [Space(15)]

    [Header("Fade In and Out")]
    [SerializeField] protected float fadeInSecsInOut;
    [SerializeField] protected float visibleSecs;
    [SerializeField] protected float fadeOutSecsInOut;

    protected CanvasGroup canvasGroup;
    private UIEffects uiEffects;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        uiEffects = new(this, canvasGroup);
        uiEffects.OnEffectComplete += InvokeOnFadeComplete;
    }

    public void FadeIn(float _secsDelay = 0) => uiEffects.FadeIn(fadeInSecs, _secsDelay: _secsDelay);
    public void FadeOut(float _secsDelay = 0) => uiEffects.FadeOut(fadeOutSecs, _secsDelay: _secsDelay);
    public void FadeInOut(float _secsDelay = 0) => uiEffects.FadeInAndOut(fadeInSecsInOut, visibleSecs, fadeOutSecsInOut, _secsDelay: _secsDelay);

    protected void InvokeOnFadeComplete() => OnFadeComplete?.Invoke();
}
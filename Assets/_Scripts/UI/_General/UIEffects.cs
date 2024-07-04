using UnityEngine;
using System;
using System.Collections;

public class UIEffects
{
    public event Action OnEffectComplete;

    private readonly MonoBehaviour coroutineStarter;
    private readonly CanvasGroup canvasGroup;

    public UIEffects(MonoBehaviour _coroutineStarter, CanvasGroup _canvasGroup)
    {
        coroutineStarter = _coroutineStarter;
        canvasGroup = _canvasGroup;
    }

    public void FadeIn(float _fadeInSecs, float _secsDelay = 0)
    {
        IEnumerator _fadeCoroutine = Co_Fade(_fadeInSecs, 1f, _secsDelay);
        coroutineStarter.StartCoroutine(_fadeCoroutine);
    }

    public void FadeOut(float _fadeOutSecs, float _secsDelay = 0)
    {
        IEnumerator _fadeCoroutine = Co_Fade(_fadeOutSecs, 0f, _secsDelay);
        coroutineStarter.StartCoroutine(_fadeCoroutine);
    }

    public void FadeInAndOut(float _fadeInSecs, float _fullyVisibleSecs, float _fadeOutSecs, float _secsDelay = 0)
    {
        IEnumerator _fadeInOutCoroutine = Co_FadeInOut(_fadeInSecs, _fullyVisibleSecs, _fadeOutSecs, _secsDelay);
        coroutineStarter.StartCoroutine(_fadeInOutCoroutine);
    }

    private IEnumerator Co_Fade(float _fadeSecs, float _alphaTarget, float _secsDelay, bool _invokeEffectComplete = true)
    {
        yield return new WaitForSeconds(_secsDelay);

        float _time = 0f;
        float _startAlpha = canvasGroup.alpha;

        while (_time / _fadeSecs < 1f)
        {
            if (canvasGroup == null)
                break;

            canvasGroup.alpha = Mathf.Lerp(_startAlpha, _alphaTarget, _time / _fadeSecs);
            _time += Time.deltaTime;

            yield return null;
        }

        canvasGroup.alpha = _alphaTarget;

        if (_invokeEffectComplete)
            OnEffectComplete?.Invoke();
    }

    private IEnumerator Co_FadeInOut(float _fadeInDuration, float _fullyVisibleDuration, float _fadeOutDuration,
                                     float _secsDelay, bool _invokeEffectComplete = true)
    {
        yield return Co_Fade(_fadeInDuration, 1f, _secsDelay, _invokeEffectComplete: false);
        yield return new WaitForSeconds(_fullyVisibleDuration);
        yield return Co_Fade(_fadeOutDuration, 0f, 0f, _invokeEffectComplete: false);

        if (_invokeEffectComplete)
            OnEffectComplete?.Invoke();
    }
}
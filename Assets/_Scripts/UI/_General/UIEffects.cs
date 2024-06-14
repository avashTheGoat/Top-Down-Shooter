using UnityEngine;
using System;
using System.Collections;

public class UIEffects
{
    public event Action<RectTransform> OnEffectComplete;

    private readonly MonoBehaviour coroutineStarter;

    public UIEffects(MonoBehaviour _coroutineStarter)
    {
        coroutineStarter = _coroutineStarter;
    }

    public void FadeIn(CanvasGroup _fadeTarget, float _fadeInDuration)
    {
        IEnumerator _fadeCoroutine = Co_Fade(_fadeTarget, _fadeInDuration, 1f);
        coroutineStarter.StartCoroutine(_fadeCoroutine);
    }

    public void FadeOut(CanvasGroup _fadeTarget, float _fadeInDuration)
    {
        IEnumerator _fadeCoroutine = Co_Fade(_fadeTarget, _fadeInDuration, 0f);
        coroutineStarter.StartCoroutine(_fadeCoroutine);
    }

    public void FadeInAndOut(CanvasGroup _fadeTarget, float _fadeInDuration,
    float _fullyVisibleDuration, float _fadeOutDuration)
    {
        IEnumerator _fadeInOutCoroutine = Co_FadeInOut(_fadeTarget, _fadeInDuration, _fullyVisibleDuration, _fadeOutDuration);
        coroutineStarter.StartCoroutine(_fadeInOutCoroutine);
    }

    private IEnumerator Co_Fade(CanvasGroup _fadeTarget, float _fadeDuration, float _alphaTarget, bool _invokeEffectComplete = true)
    {
        float _time = 0f;
        float _startAlpha = _fadeTarget.alpha;

        while (_time / _fadeDuration < 1f)
        {
            if (_fadeTarget == null)
                break;

            _fadeTarget.alpha = Mathf.Lerp(_startAlpha, _alphaTarget, _time / _fadeDuration);
            _time += Time.deltaTime;

            yield return null;
        }

        _fadeTarget.alpha = _alphaTarget;

        if (_invokeEffectComplete)
            OnEffectComplete?.Invoke(_fadeTarget.GetComponent<RectTransform>());
    }

    private IEnumerator Co_FadeInOut(CanvasGroup _fadeTarget, float _fadeInDuration,
    float _fullyVisibleDuration, float _fadeOutDuration)
    {
        yield return Co_Fade(_fadeTarget, _fadeInDuration, 1f, _invokeEffectComplete: false);
        yield return new WaitForSeconds(_fullyVisibleDuration);
        yield return Co_Fade(_fadeTarget, _fadeOutDuration, 0f, _invokeEffectComplete: false);

        OnEffectComplete?.Invoke(_fadeTarget.GetComponent<RectTransform>());
    }
}
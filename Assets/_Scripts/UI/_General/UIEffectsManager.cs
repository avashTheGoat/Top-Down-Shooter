using UnityEngine;
using System.Collections;

public class UIEffectsManager : MonoBehaviour
{
    public static UIEffectsManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
            Destroy(gameObject);
    }

    public void FadeIn(CanvasGroup _fadeTarget, float _fadeInDuration)
    {
        IEnumerator _fadeCoroutine = Co_Fade(_fadeTarget, _fadeInDuration, 1f);
        StartCoroutine(_fadeCoroutine);
    }

    public void FadeOut(CanvasGroup _fadeTarget, float _fadeInDuration)
    {
        IEnumerator _fadeCoroutine = Co_Fade(_fadeTarget, _fadeInDuration, 0f);
        StartCoroutine(_fadeCoroutine);
    }

    public void FadeInAndOut(CanvasGroup _fadeTarget, float _fadeInDuration,
    float _fullyVisibleDuration, float _fadeOutDuration)
    {
        IEnumerator _fadeInOutCoroutine = Co_FadeInOut(_fadeTarget, _fadeInDuration, _fullyVisibleDuration, _fadeOutDuration);
        StartCoroutine(_fadeInOutCoroutine);
    }

    private IEnumerator Co_Fade(CanvasGroup _fadeTarget, float _fadeDuration, float _alphaTarget)
    {
        float _time = 0f;
        float _startAlpha = _fadeTarget.alpha;

        while (_time / _fadeDuration < 1f)
        {
            _fadeTarget.alpha = Mathf.Lerp(_startAlpha, _alphaTarget, _time / _fadeDuration);
            _time += Time.deltaTime;

            yield return null;
        }

        _fadeTarget.alpha = _alphaTarget;
    }

    private IEnumerator Co_FadeInOut(CanvasGroup _fadeTarget, float _fadeInDuration,
    float _fullyVisibleDuration, float _fadeOutDuration)
    {
        yield return Co_Fade(_fadeTarget, _fadeInDuration, 1f);
        yield return new WaitForSeconds(_fullyVisibleDuration);
        yield return Co_Fade(_fadeTarget, _fadeInDuration, 0f);
    }
}
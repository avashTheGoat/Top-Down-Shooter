using System;

public class Timer
{
    public event Action<Timer> OnTick;
    public event Action OnComplete;

    public float TimeElapsed { get; private set; } = 0f;

    private float maxTime = float.MinValue;

    public Timer() { }

    public Timer(float _maxTime)
    {
        if (_maxTime <= 0)
            throw new ArgumentException("_maxTime cannot be less than or equal to 0");

        maxTime = _maxTime;
    }

    public void Tick(float _deltaTime)
    {
        TimeElapsed += _deltaTime;
        OnTick?.Invoke(this);

        if (maxTime == float.MinValue)
            return;

        if (GetRemainingTime() == 0f)
            OnComplete?.Invoke();
    }

    public float GetRemainingTime()
    {
        if (maxTime == float.MinValue)
            return float.MinValue;

        return maxTime - TimeElapsed < 0 ? 0 : maxTime - TimeElapsed;
    }

    public void Reset() => TimeElapsed = 0f; 
}
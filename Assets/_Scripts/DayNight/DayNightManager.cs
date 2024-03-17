using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// requires DayNightCycle to make night at "edges" of progress (ie. closer to 0/1)
// and day at the middle of progress
public class DayNightManager : MonoBehaviour
{
    public event Action OnDayEnd;
    public event Action OnNightEnd;

    public bool ShouldUpdate = true;

    [SerializeField] private float dayTimeSecondsLength;

    [Header("Delta Night Time (when enemy dies/spawns)")]
    [SerializeField] private float deltaNightTimePerSec;
    [Space(15)]

    [Header("References")]
    [SerializeField] private DayNightCycle dayNightCycle;
    [SerializeField] private EnemyWavesController enemyWaves;
    [Space(15)]

    [Header("Night Info")]
    [SerializeField] private float NIGHT_START;
    [SerializeField] private float NIGHT_END;

    private Timer dayTimeTimer;
    private bool isFirstDayTime = true;

    private float newNightTimeProgress;
    private bool hasNightTimeCoroutineBeenStarted = false;
    private IEnumerator nightLerpEnumerator;
    private List<GameObject> subscribedEnemies = new();

    private void Awake()
    {
        nightLerpEnumerator = Co_LerpNightTime();

        dayTimeTimer = new(dayTimeSecondsLength);
        dayTimeTimer.OnTick += UpdateDayTime;
        dayTimeTimer.OnComplete += () => OnDayEnd?.Invoke();
        dayTimeTimer.OnComplete += () => dayNightCycle.SetDayPercentProgress(NIGHT_START);
    }

    private void Start() => enemyWaves.OnEnemySpawn += UpdateNightTime;

    private void Update()
    {
        if (!ShouldUpdate)
            return;

        if (IsNight())
        {
            isFirstDayTime = true;

            // guards against OnDayEnd subscribers taking too long to activate enemy wave
            // and also useful for testing
            if (!enemyWaves.isActiveAndEnabled)
                return;

            foreach (var _enemy in enemyWaves.SpawnedEnemies)
            {
                if (subscribedEnemies.Contains(_enemy))
                    continue;

                _enemy.GetComponent<IKillable>().OnKill += _ => UpdateNightTime();
                subscribedEnemies.Add(_enemy);
            }
        }

        else
        {
            if (isFirstDayTime)
            {
                dayNightCycle.SetDayPercentProgress(NIGHT_END);
                OnNightEnd?.Invoke();

                dayTimeTimer.Reset();
                subscribedEnemies = new();

                hasNightTimeCoroutineBeenStarted = false;
                isFirstDayTime = false;
            }

            dayTimeTimer.Tick(Time.deltaTime);
        }
    }

    public bool IsNight() => dayNightCycle.DayPercentProgress >= NIGHT_START || dayNightCycle.DayPercentProgress < NIGHT_END;

    public float GetRemainingDayTime()
    {
        if (IsNight())
            return -1f;

        return dayTimeTimer.GetRemainingTime();
    }

    public void SubscribeToDayTimerOnTick(Action<Timer> _func) => dayTimeTimer.OnTick += _func;

    private void UpdateDayTime(Timer _timer)
    {
        float _dayProgress = _timer.TimeElapsed / dayTimeSecondsLength * (NIGHT_START - NIGHT_END);
        dayNightCycle.SetDayPercentProgress(NIGHT_END + _dayProgress);
    }

    private void UpdateNightTime()
    {
        float _totalNightProgress = GetNightProgress();
        newNightTimeProgress = (NIGHT_START + _totalNightProgress) % (1f + Mathf.Epsilon);

        if (!hasNightTimeCoroutineBeenStarted)
        {
            StartCoroutine(nightLerpEnumerator);
            nightLerpEnumerator = Co_LerpNightTime();
            hasNightTimeCoroutineBeenStarted = true;
        }
    }

    // takes both num enemies spawned and num enemies killed into account
    private float GetNightProgress()
    {
        float _nightProgress = enemyWaves.TotalNumSpawnedEnemies / enemyWaves.TotalEnemies * (1f - (NIGHT_START - NIGHT_END)) * 0.5f;
        _nightProgress += enemyWaves.NumKilledEnemies / enemyWaves.TotalEnemies * (1f - (NIGHT_START - NIGHT_END)) * 0.5f;

        return _nightProgress;
    }

    private IEnumerator Co_LerpNightTime()
    {
        float _time = dayNightCycle.DayPercentProgress;
        bool _isTimeGreater = _time > newNightTimeProgress;

        while (true)
        {
            bool _shouldBreak = false;
            // works for case where time is greater than new time
            // because it stops when the time goes from being less than
            // old time to being greater than new time
            // works for case where time is less than new time
            // because it goes until time is greater than new time
            if (_isTimeGreater)
                _isTimeGreater = _time > newNightTimeProgress;

            if (!_isTimeGreater)
            {
                if (_time > newNightTimeProgress)
                    _shouldBreak = true;
            }

            _time += deltaNightTimePerSec * Time.deltaTime;
            _time %= 1f;
            
            dayNightCycle.SetDayPercentProgress(_time);

            if (_shouldBreak)
                break;

            yield return null;
        }

        hasNightTimeCoroutineBeenStarted = false;
    }
}
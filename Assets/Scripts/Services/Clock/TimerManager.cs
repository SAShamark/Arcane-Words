using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Clock
{
    public class TimerManager
    {
        private readonly MonoBehaviour _monoBehaviour;
        private readonly Dictionary<string, Coroutine> _timers = new();

        public TimerManager(MonoBehaviour monoBehaviour)
        {
            _monoBehaviour = monoBehaviour;
        }

        public void StartTimer(string timerId, float durationInSeconds, Action onTimerComplete)
        {
            if (_timers.ContainsKey(timerId))
            {
                StopTimer(timerId);
            }

            Coroutine timerCoroutine =
                _monoBehaviour.StartCoroutine(TimerCoroutine(timerId, durationInSeconds, onTimerComplete));
            _timers[timerId] = timerCoroutine;
        }

        public void StopTimer(string timerId)
        {
            if (_timers.TryGetValue(timerId, out Coroutine timerCoroutine))
            {
                _monoBehaviour.StopCoroutine(timerCoroutine);
                _timers.Remove(timerId);
            }
        }

        public void StopAllTimers()
        {
            foreach (Coroutine timer in _timers.Values)
            {
                _monoBehaviour.StopCoroutine(timer);
            }

            _timers.Clear();
        }

        private IEnumerator TimerCoroutine(string timerId, float durationInSeconds, Action onTimerComplete)
        {
            yield return new WaitForSeconds(durationInSeconds);

            onTimerComplete?.Invoke();
            _timers.Remove(timerId);
        }
    }
}
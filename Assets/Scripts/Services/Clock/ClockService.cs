using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Clock
{
    public class ClockService : MonoBehaviour, IClockService
    {
        private StopwatchManager _stopwatchManager;
        private TimerManager _timerManager;

        private void Awake()
        {
            _stopwatchManager = new StopwatchManager();
            _timerManager = new TimerManager(this);
        }

        public Dictionary<string, StopwatchManager.StopwatchData> Stopwatches => _stopwatchManager.Stopwatches;
        public void StartStopwatch(string timerId) => _stopwatchManager.StartStopwatch(timerId);
        public float StopStopwatch(string timerId) => _stopwatchManager.StopStopwatch(timerId);
        public void ResetStopwatch(string timerId) => _stopwatchManager.ResetStopwatch(timerId);
        public float CalculateElapsedTime(string timerId) => _stopwatchManager.CalculateElapsedTime(timerId);

        public void StartTimer(string timerId, float durationInSeconds, Action onTimerComplete) =>
            _timerManager.StartTimer(timerId, durationInSeconds, onTimerComplete);
        public void StopTimer(string timerId) => _timerManager.StopTimer(timerId);
        public void StopAllTimers() => _timerManager.StopAllTimers();

        public string FormatToTime(float timeInSeconds, bool includeHours = false)
        {
            int hours = Mathf.FloorToInt(timeInSeconds / ValueConstants.SECONDS_IN_HOUR);
            int minutes = Mathf.FloorToInt(timeInSeconds % ValueConstants.SECONDS_IN_HOUR / ValueConstants.SECONDS_IN_MINUTE);
            int seconds = Mathf.FloorToInt(timeInSeconds % ValueConstants.SECONDS_IN_MINUTE);

            return includeHours ? $"{hours:00}:{minutes:00}:{seconds:00}" : $"{minutes:00}:{seconds:00}";
        }
    }
}
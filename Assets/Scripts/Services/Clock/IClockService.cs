using System;
using System.Collections.Generic;

namespace Services.Clock
{
    public interface IClockService
    {
        public Dictionary<string, StopwatchManager.StopwatchData> Stopwatches { get; }
        void StartStopwatch(string timerId);
        float StopStopwatch(string timerId);
        void ResetStopwatch(string timerId);
        float CalculateElapsedTime(string timerId);
        void StartTimer(string timerId, float durationInSeconds, Action onTimerComplete);
        public void StopTimer(string timerId);
        public void StopAllTimers();
        public string FormatToTime(float timeInSeconds, bool includeHours = false);
    }
}
using System;
using System.Collections.Generic;

namespace Services.Clock
{
    public class ClockService : IClockService
    {
        public class TimerData
        {
            public DateTime StartTime { get; set; }
            public TimeSpan ElapsedTime { get; set; }
            public bool IsRunning { get; set; }
        }

        public Dictionary<string, TimerData> Timers { get; private set; } = new();
        
        public void StartStopwatch(string timerId)
        {
            if (!Timers.ContainsKey(timerId))
            {
                Timers[timerId] = new TimerData();
            }

            TimerData timerData = Timers[timerId];
            timerData.StartTime = DateTime.Now;
            timerData.IsRunning = true;
        }

        public float StopStopwatch(string timerId)
        {
            if (!Timers.ContainsKey(timerId) || !Timers[timerId].IsRunning)
            {
                throw new InvalidOperationException($"Stopwatch '{timerId}' is not running.");
            }

            TimerData timerData = Timers[timerId];
            timerData.IsRunning = false;

            return CalculateElapsedTime(timerId);
        }

        public void ResetStopwatch(string timerId)
        {
            if (Timers.ContainsKey(timerId))
            {
                Timers[timerId] = new TimerData { StartTime = DateTime.MinValue, ElapsedTime = TimeSpan.Zero };
            }
        }

        public float CalculateElapsedTime(string timerId)
        {
            if (!Timers.ContainsKey(timerId))
            {
                throw new InvalidOperationException($"Timer '{timerId}' not found.");
            }

            TimerData timerData = Timers[timerId];
            DateTime endTime = DateTime.Now;
            timerData.ElapsedTime = endTime - timerData.StartTime;
            float elapsedTimeInSeconds =
                (float)timerData.ElapsedTime.TotalMilliseconds / ValueConstants.MILLISECONDS_IN_SECOND;
            return elapsedTimeInSeconds;
        }
    }
}
using System;
using System.Collections.Generic;

namespace Services.Clock
{
    public class StopwatchManager
    {
        public class StopwatchData
        {
            public DateTime StartTime { get; set; }
            public TimeSpan ElapsedTime { get; set; }
            public bool IsRunning { get; set; }
        }

        public Dictionary<string, StopwatchData> Stopwatches { get; private set; } = new();

        public void StartStopwatch(string timerId)
        {
            if (!Stopwatches.ContainsKey(timerId))
            {
                Stopwatches[timerId] = new StopwatchData();
            }

            StopwatchData stopwatchData = Stopwatches[timerId];
            stopwatchData.StartTime = DateTime.Now;
            stopwatchData.IsRunning = true;
        }

        public float StopStopwatch(string timerId)
        {
            if (!Stopwatches.ContainsKey(timerId) || !Stopwatches[timerId].IsRunning)
            {
                throw new InvalidOperationException($"Stopwatch '{timerId}' is not running.");
            }

            StopwatchData stopwatchData = Stopwatches[timerId];
            stopwatchData.IsRunning = false;

            return CalculateElapsedTime(timerId);
        }

        public void ResetStopwatch(string timerId)
        {
            if (Stopwatches.ContainsKey(timerId))
            {
                Stopwatches[timerId] = new StopwatchData { StartTime = DateTime.MinValue, ElapsedTime = TimeSpan.Zero };
            }
        }

        public float CalculateElapsedTime(string timerId)
        {
            if (!Stopwatches.ContainsKey(timerId))
            {
                throw new InvalidOperationException($"Timer '{timerId}' not found.");
            }

            StopwatchData stopwatchData = Stopwatches[timerId];
            DateTime endTime = DateTime.Now;
            stopwatchData.ElapsedTime = endTime - stopwatchData.StartTime;
            float elapsedTimeInSeconds =
                (float)stopwatchData.ElapsedTime.TotalMilliseconds / ValueConstants.MILLISECONDS_IN_SECOND;
            return elapsedTimeInSeconds;
        }
    }
}
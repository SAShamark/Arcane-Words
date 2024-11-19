using System.Collections.Generic;

namespace Services.Clock
{
    public interface IClockService
    {
        public Dictionary<string, ClockService.TimerData> Timers { get; }
        void StartStopwatch(string timerId);
        float StopStopwatch(string timerId);
        void ResetStopwatch(string timerId);
        float CalculateElapsedTime(string timerId);

    }
}
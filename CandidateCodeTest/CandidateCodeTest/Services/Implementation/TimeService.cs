using CandidateCodeTest.Services.Interfaces;
using System;

namespace CandidateCodeTest.Services.Implementation
{
    internal class TimeService : ITimeService
    {
        private readonly ILogger _logger;

        public TimeService(ILogger logger)
        {
            _logger = logger;
        }

        public bool IsWithinBusinessHours()
        {
            TimeSpan start = new TimeSpan(10, 0, 0);
            TimeSpan end = new TimeSpan(12, 0, 0);
            TimeSpan now = DateTime.Now.TimeOfDay;

            bool isWithinHours = now > start && now < end;

            _logger.Log($"Current time {now}, withing business hours: {isWithinHours}");

            return isWithinHours;
        }
    }
}

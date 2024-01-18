using CandidateCodeTest.Services.Interfaces;
using System;

namespace CandidateCodeTest.Services.Implementation
{
    public class CustomerService
    {
        private readonly IEmailService _emailService;
        private readonly ITimeService _timeService;
        private readonly ILogger _logger;

        public CustomerService(IEmailService emailService, ITimeService timeService, ILogger logger)
        {
            _emailService = emailService;
            _timeService = timeService;
            _logger = logger;
        }

        public bool HasEmailBeenSent()
        {
            if (_timeService.IsWithinBusinessHours())
            {
                try
                {
                    _emailService.SendEmail("dummy.email@test.com", "Subject", "Hello");
                    _logger.Log("Email sent during business hours");
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.Log("Error sending email: " + ex.Message);
                    return false;
                }
            }

            _logger.Log("Sending email outside business hours");
            return false;
        }
    }
}

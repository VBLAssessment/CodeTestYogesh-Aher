using CandidateCodeTest.Services.Implementation;
using CandidateCodeTest.Services.Interfaces;
using Moq;
using Xunit;

namespace CandidateCodeTest_UnitTests
{
    public class CustomerServiceTests
    {
        private readonly CustomerService _customerService;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly Mock<ILogger> _mockLogger;

        private readonly string toEmail = "dummy.email@test.com";
        private readonly string subject = "Subject";
        private readonly string body = "Hello"; 
        
        public CustomerServiceTests()
        {
            _mockEmailService = new Mock<IEmailService>();
            _mockTimeService = new Mock<ITimeService>();
            _mockLogger = new Mock<ILogger>();
            _customerService = new CustomerService(_mockEmailService.Object, _mockTimeService.Object, _mockLogger.Object);
        }

        [Fact]
        public void Within_Time_Window_Email_Has_Been_Sent()
        {
            _mockTimeService.Setup(t => t.IsWithinBusinessHours()).Returns(true);

            var result = _customerService.HasEmailBeenSent();

            Assert.True(result);
            _mockEmailService.Verify(m => m.SendEmail(toEmail, subject, body), Times.Once);
            _mockLogger.Verify(l => l.Log("Email sent during business hours"), Times.Once);
        }

        [Fact]
        public void Outside_Time_Window_Email_Has_Not_Been_Sent()
        {
            _mockTimeService.Setup(t => t.IsWithinBusinessHours()).Returns(false);

            var result = _customerService.HasEmailBeenSent();

            Assert.False(result);
            _mockEmailService.Verify(m => m.SendEmail(toEmail, subject, body), Times.Never);
            _mockLogger.Verify(l => l.Log("Sending email outside business hours"), Times.Once);
        }

        [Fact]
        public void Email_Sent_At_Start_Of_Business_Hours()
        {
            _mockTimeService.Setup(t => t.IsWithinBusinessHours()).Returns(true);

            bool result = _customerService.HasEmailBeenSent();

            Assert.True(result);
            _mockEmailService.Verify(m => m.SendEmail(toEmail, subject, body), Times.Once);
            _mockLogger.Verify(l => l.Log(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public void Email_Not_Sent_At_End_Of_Business_Hours()
        {
            _mockTimeService.Setup(t => t.IsWithinBusinessHours()).Returns(false);

            bool result = _customerService.HasEmailBeenSent();

            Assert.False(result);
            _mockEmailService.Verify(m => m.SendEmail(toEmail, subject, body), Times.Never);
            _mockLogger.Verify(l => l.Log(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public void Email_Sending_Throws_Exception_Is_Handled()
        {
            _mockTimeService.Setup(t => t.IsWithinBusinessHours()).Returns(true);
            _mockEmailService.Setup(m => m.SendEmail(toEmail, subject, body)).Throws(new Exception("Email sending failed"));

            var exception = Record.Exception(() => _customerService.HasEmailBeenSent());
            Assert.Null(exception);
            _mockLogger.Verify(l => l.Log("Error sending email: Email sending failed"), Times.Once);
        }
    }
}

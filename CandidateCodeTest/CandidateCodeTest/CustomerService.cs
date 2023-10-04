using System;

namespace CandidateCodeTest
{
    public class CustomerService
    {
        public bool HasEmailBeenSent()
        {
            MessageService messageService = new MessageService();

            TimeSpan start = new TimeSpan(10, 0, 0); 
            TimeSpan end = new TimeSpan(12, 0, 0); 
            TimeSpan now = DateTime.Now.TimeOfDay;

            if ((now > start) && (now < end))
            {
                messageService.SendEmail();
                return true;
            }
            else
                return false;
        }
    }

    public class MessageService
    {
        public void SendEmail()
        {
            // Code that will send the email
            Console.WriteLine("Email Sent to customer");
        }
    }
}

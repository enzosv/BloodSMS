using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace Blood_SMS
{
    class Email
    {
        MailAddress fromAddress;
        MailAddress toAddress;
        SmtpClient smtp;
        public Email(string recipient, string subject, string body)
        {
           fromAddress = new MailAddress("eamc.bloodbank@gmail.com");
           toAddress = new MailAddress(recipient);

           smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, "bloodbus")
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
           
    }
}

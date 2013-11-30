using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter recipient email");
            string recipient = Console.ReadLine();

            Console.WriteLine("Enter Subject :");
            string subject = Console.ReadLine();
            Console.WriteLine("Enter message :");
            string body = Console.ReadLine();

            var fromAddress = new MailAddress("eamc.bloodbank@gmail.com");
            var toAddress = new MailAddress(recipient);

            var smtp = new SmtpClient
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
            Console.Read();
        }
    }
}
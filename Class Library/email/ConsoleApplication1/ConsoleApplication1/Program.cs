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
            Console.WriteLine("Enter email:");
            string email = Console.ReadLine();
            Console.WriteLine("Enter password");
            string password = Console.ReadLine();
            NetworkCredential cred = new NetworkCredential(email, password);

            MailMessage msg = new MailMessage();
            Console.WriteLine("Enter Recipient: ");
            string recipient = Console.ReadLine();
            msg.To.Add(recipient);
            msg.From = new MailAddress(email);
            msg.Subject = "A subject.";
            msg.Body = "Hello, this is my message.";

            //NetworkCredential cred = new NetworkCredential("prankster@gmail.com", "EmailAccountPass");
            //msg.From = new MailAddress("CrazyGuy@insane.com");

            SmtpClient client = new SmtpClient("smtp.gmail.com", 25);

            client.Credentials = cred; // Send our account login details to the client.
            client.EnableSsl = true;   // Read below.
            client.Send(msg);          // Send our email.

            // Host List:
            // smtp.gmail.com // Gmail
            // smtp.live.com // Windows live / Hotmail
            // smtp.mail.yahoo.com // Yahoo
            // smtp.aim.com // AIM
            // my.inbox.com // Inbox
            Console.Read();
        }
    }
}
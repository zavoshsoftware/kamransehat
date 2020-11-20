using System.Net;
using System.Net.Mail;

namespace Helper
{
    public class Message
    {
        public void Send(string reciever,string subject, string message,string messageType)
        {
            if (messageType == "email")
                SendEmail(reciever,subject,message);
        }

        public void SendEmail(string reciever,string subject, string message)
        {
           // SmtpClient client = new SmtpClient("https://ghanongostar.com/webmail");

            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "209.44.107.142";
            client.Port = 25;

            //If you need to authenticate
            client.Credentials = new NetworkCredential("info@kamransehat.ir", "123qwe!@#QWE");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("info@kamransehat.ir");
            mailMessage.To.Add(reciever);
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            
            mailMessage.IsBodyHtml = true;

            client.Send(mailMessage);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace MiniDropbox.Web.Utils
{
    public class MailSender
    {
            public static bool SendEmail(string addresses,string subject,string body)
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("postmaster@app1561.mailgun.org", "MiniDropBox")
                };

                mailMessage.To.Add(addresses);
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = subject;

                mailMessage.Priority = MailPriority.Normal;
                var smtp = new SmtpClient("smtp.mailgun.org")
                {
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                    Credentials = new System.Net.NetworkCredential("postmaster@app1561.mailgun.org", "70ic5h7hd6z2"),
                    Host = "smtp.mailgun.org",
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                try
                {
                    smtp.Send(mailMessage);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
    }
}
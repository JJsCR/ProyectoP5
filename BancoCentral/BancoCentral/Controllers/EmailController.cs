using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace BancoCentral.Controllers
{
    public class EmailController : Controller
    {
        public void sendEmail(String asunto, String body, String correo)
        {
            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(correo));
            email.From = new MailAddress("mepcostaricaprueba@hotmail.com");
            email.Subject = asunto;
            email.Body = body;
            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;

            string output = null;
            SmtpClient smtp = instanciaSmtp();

            try
            {
                smtp.Send(email);

                email.Dispose();
            }
            catch (Exception ex)
            {
                output = "Error enviando correo electrónico: " + ex.Message;
            }

        }

        public SmtpClient instanciaSmtp()
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp-mail.outlook.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("mepcostaricaprueba@hotmail.com", "prueba2019");

            return smtp;
        }
    }
}
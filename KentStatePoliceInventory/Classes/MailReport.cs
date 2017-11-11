using System.Net.Mail;
using System;

namespace KentStatePoliceInventory.Classes
{
    public class MailReport
    {
        public MailReport()
        {

        }

        public bool SendReport(string reportLocation)
        {
            MailMessage mail = new MailMessage();
            var config = new Configuration();
            SmtpClient SmtpServer = new SmtpClient("smtp.kent.edu");

            mail.From = new MailAddress(config.FromAddress);
            foreach (var address in config.ToAddresses)
            {
                mail.To.Add(address);
            }

            mail.Subject = "KSU Police Department Report";
            mail.Body = "Attached is a copy of your report that you have generated";
            mail.Attachments.Add(new Attachment(reportLocation));

            SmtpServer.Port = 25;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential(config.MailUser, config.MailPassword, "smtp.kent.edu");

            try
            {
                SmtpServer.Send(mail);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
    }
}
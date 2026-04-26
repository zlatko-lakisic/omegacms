using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.Helpers.Mailer;
using MD.CMS.BusinessLogic.Core.Properties;
using MD.Tools.Helpers.Core.Net.Email;
using MD.Tools.Helpers.Core;
using System;
using System.Linq;
using System.Net.Mail;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using System.Security.Cryptography;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public class EmailController : BaseController<EmailController>
    {
        public bool SendMail(string recipient)
        {
            /*User user = MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).GetIdByUserName(recipient);
            long idUser = user.Id;
            string token1 = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());
            byte[] data = Convert.FromBase64String(token);

            MD5 md5 = new MD5CryptoServiceProvider();
            Byte[] originalBytes = ASCIIEncoding.ASCII.GetBytes(token1);
            Byte[] encodedBytes = md5.ComputeHash(originalBytes);

            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in encodedBytes)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string token2 = s.ToString();

            string putanja = "https://example.com/admin/changepassword?token=" + token2;

            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            string datumTokena = when.ToString("yyyy-MM-dd HH:mm:ss ");

            User updateUser = MD.CMS.BusinessLogic.Core.DataAccess.Controllers.UserController.GetNewInstance().DefaultPlugin(this.UseDefaultPlugin).Caller(UserMakingTheCall).UpdateUser(idUser, token2, datumTokena);

            bool suceess = false;
            string msg = string.Empty;

            EmailContent[] emailContent = null;
            emailContent = new EmailContent[1];
            emailContent[0] = new HtmlContent("Link for change password;    " + putanja + "     ;", Encoding.ASCII);

            EmailService.EmailSender = new MDMailer()
             {
                 EnableTls = Settings.Default.EmailEnableSsl,
                 SenderAddress = Settings.Default.Username,
                 SenderPassword = Settings.Default.Password,
                 SmtpPort = Settings.Default.EmailPort,
                 SmtpServer = Settings.Default.EmailHost
             };

            string subject = "subject";
            string body = "body";

            if (when > DateTime.UtcNow.AddHours(-24))
            {
                try
                {
                    EmailService.Send(new System.Net.Mail.MailAddress(Settings.Default.Username),
                        new System.Net.Mail.MailAddress(recipient), subject, emailContent);
                    suceess = true;

                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    suceess = false;

                }
            }
            else
            {
                msg = "Token has expired";
            }
            return suceess;
            */
            return true;


        }


        public bool MailSend(string recipient, string messageSubject, string messageBody, Attachment attachment = null)
        {
            bool success = false;
            string body = messageBody;
            string subject = messageSubject;            
            EmailContent[] emailContent = null;
            emailContent = new EmailContent[1];
            emailContent[0] = new HtmlContent(body, Encoding.ASCII);           

            EmailService.EmailSender = new MDMailer()
            {
                EnableTls = Settings.Default.EmailEnableSsl,
                SenderAddress = Settings.Default.Username,
                SenderPassword = Settings.Default.Password,
                SmtpPort = Settings.Default.EmailPort,
                SmtpServer = Settings.Default.EmailHost
            };

            try
            {
                EmailService.Send(new System.Net.Mail.MailAddress(Settings.Default.Username),
                    new System.Net.Mail.MailAddress(recipient), subject, emailContent, attachment);
                success = true;

            }
            catch (Exception ex)
            {
                MD.Tools.Helpers.Core.Logging.Logger.Log(ex);
                success = false;

            }

            return success;
        }
    }
}

using Model.Domain.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace Db.Persistence.Services
{
    public class EmailService
    {
        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="emailAccount">Email account to use</param>
        /// <param name="subject">Subject</param>
        /// <param name="body">Body</param>
        /// <param name="toAddress">To address</param>
        /// <param name="toName">To display name</param>
        /// <param name="fromAddress">From address</param>
        /// <param name="fromName">From display name</param>
        /// <param name="replyTo">ReplyTo address</param>
        /// <param name="replyToName">ReplyTo display name</param>
        /// <param name="bcc">BCC addresses list</param>
        /// <param name="cc">CC addresses list</param>
        /// <param name="headers">Headers</param>

        public EmailService()
        {

        }


        public static string SendEmail(EmailAccount emailAccount, string subject, string body,
                string toAddress, string toName, string fromAddress = null,Attachment attachment = null, string fromName = null,
                 string replyTo = null, string replyToName = null,
                IEnumerable<string> bcc = null, IEnumerable<string> cc = null,
                IDictionary<string, string> headers = null)
        {

            var message = new MailMessage();
            string Ok = "Ok";
            //from, to, reply to
            fromAddress = fromAddress ?? emailAccount.Email;
            fromName = fromName ?? emailAccount.DisplayName;
            message.From = new MailAddress(fromAddress, fromName);
            message.To.Add(new MailAddress(toAddress, toName));
            if (!String.IsNullOrEmpty(replyTo))
            {
                message.ReplyToList.Add(new MailAddress(replyTo, replyToName));
            }

            //BCC
            if (bcc != null)
            {
                foreach (var address in bcc.Where(bccValue => !String.IsNullOrWhiteSpace(bccValue)))
                {
                    message.Bcc.Add(address.Trim());
                }
            }
          
            //CC
            if (cc != null)
            {
                foreach (var address in cc.Where(ccValue => !String.IsNullOrWhiteSpace(ccValue)))
                {
                    message.CC.Add(address.Trim());
                }
            }

            //content
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            // Attachements
            if(attachment != null)
                message.Attachments.Add(attachment);
            

            //headers
            if (headers != null)
                foreach (var header in headers)
                {
                    message.Headers.Add(header.Key, header.Value);
                }

            //create the file attachment for this e-mail message
           
            //send email
            try
            {
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.UseDefaultCredentials = emailAccount.UseDefaultCredentials;
                    smtpClient.Host = emailAccount.Host;
                    smtpClient.Port = emailAccount.Port;
                    smtpClient.EnableSsl = emailAccount.EnableSsl;
                    smtpClient.Credentials = emailAccount.UseDefaultCredentials ? CredentialCache.DefaultNetworkCredentials : new NetworkCredential(emailAccount.Username, emailAccount.Password);
                    smtpClient.Send(message);
                }
            }
            
            catch (Exception ex)
            {
                if (attachment != null)
                    attachment.Dispose();
                Ok = "Error";
            }

            return Ok;
        }
    }
}
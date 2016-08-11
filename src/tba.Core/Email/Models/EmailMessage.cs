using System.Net.Mail;

namespace tba.Core.Email.Models
{
    public class EmailMessage
    {
        /// <summary>
        /// Must be a valid email address
        /// </summary>
        public MailAddress[] ToEmails { get; set; }

        /// <summary>
        /// Give a name to the recipient.
        /// </summary>
        public string[] ToNames { get; set; }

        /// <summary>
        /// Must be a valid email address, copy
        /// </summary>
        public MailAddress[] CcEmails { get; set; }

        /// <summary>
        /// Must be a valid email address, blind copy
        /// </summary>
        public MailAddress[] BccEmails { get; set; }

        /// <summary>
        /// An internal category assigned to the email
        /// </summary>
        public EmailCategory Category { get; set; }

        /// <summary>
        /// The subject of the email
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The body of the email
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// If the body is in HTML format, true
        /// </summary>
        public bool IsBodyHtml { get; set; }

        /// <summary>
        /// This is where the email will appear to originate from for the recipient
        /// </summary>
        public MailAddress FromEmail { get; set; }

        /// <summary>
        /// Give a name to originate from for the recipient
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        /// Append a reply-to field to the email message
        /// </summary>
        public MailAddress ReplyToEmail { get; set; }
    }
}
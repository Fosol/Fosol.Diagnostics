using Fosol.Common.Parsers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
    /// <summary>
    /// Provides a way to send a TraceEvent over email.
    /// </summary>
    public sealed class MailListener
        : TraceListener
    {
        #region Variables
        private string _SmtpHost;
        private int _SmtpPort;
        private bool _EnableSsl;
        private int _Timeout;
        private SmtpAuthenticationMode _SmtpAuthentication;
        private string _UserName;
        private string _Password;
        private Format _From;
        private Format _To;
        private Format _CC;
        private Format _BCC;
        private Format _Subject;
        private Format _Body;
        private bool _IsBodyHtml;
        private MailPriority _Priority;

        /// <summary>
        /// Authentication options.
        /// </summary>
        public enum SmtpAuthenticationMode
        {
            /// <summary>
            /// No authentication required.
            /// </summary>
            None,
            /// <summary>
            /// Basic username and password required.
            /// </summary>
            Basic,
            /// <summary>
            /// NTLM authentication required.
            /// </summary>
            Ntlm
        }
        #endregion

        #region Properties
        /// <summary>
        /// get/set - Smtp client host name.
        /// </summary>
        [TraceSetting("Host")]
        [Required(AllowEmptyStrings = false)]
        public string SmtpHost
        {
            get { return _SmtpHost; }
            set { _SmtpHost = value; }
        }

        /// <summary>
        /// get/set - Smtp client port number.
        /// </summary>
        [DefaultValue(25)]
        [TraceSetting("Port")]
        public int SmtpPort
        {
            get { return _SmtpPort; }
            set { _SmtpPort = value; }
        }

        /// <summary>
        /// get/set - Controls whether the Smtp client will use SSL.
        /// </summary>
        [DefaultValue(false)]
        [TraceSetting("EnableSsl")]
        public bool EnableSsl
        {
            get { return _EnableSsl; }
            set { _EnableSsl = value; }
        }

        /// <summary>
        /// get/set - Number of seconds before the Smtp client will timeout.
        /// </summary>
        [TraceSetting("Timeout")]
        public int Timeout
        {
            get { return _Timeout; }
            set { _Timeout = value; }
        }

        /// <summary>
        /// get/set - Smtp client authentication mode.
        /// </summary>
        [DefaultValue(SmtpAuthenticationMode.None)]
        [TraceSetting("Authentication", typeof(EnumConverter), typeof(SmtpAuthenticationMode))]
        public SmtpAuthenticationMode SmtpAuthentication
        {
            get { return _SmtpAuthentication; }
            set { _SmtpAuthentication = value; }
        }

        /// <summary>
        /// get/set - User name when using SmtpAuthenticationMode.Basic.
        /// </summary>
        [TraceSetting("UserName")]
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        /// <summary>
        /// get/set - Password when using SmtpAuthenticationMode.Basic.
        /// </summary>
        [TraceSetting("Password")]
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        /// <summary>
        /// get/set - Who the mail is from.
        /// </summary>
        [TraceSetting("From", typeof(Fosol.Common.Parsers.Converters.FormatConverter))]
        [Required(AllowEmptyStrings = false)]
        public Format From
        {
            get { return _From; }
            set { _From = value; }
        }

        /// <summary>
        /// get/set - Who the mail is being sent to.
        /// </summary>
        [TraceSetting("To", typeof(Fosol.Common.Parsers.Converters.FormatConverter))]
        [Required(AllowEmptyStrings = false)]
        public Format To
        {
            get { return _To; }
            set { _To = value; }
        }

        /// <summary>
        /// get/set - Who the mail is being cc'ed to.
        /// </summary>
        [TraceSetting("CC", typeof(Fosol.Common.Parsers.Converters.FormatConverter))]
        public Format CC
        {
            get { return _CC; }
            set { _CC = value; }
        }

        /// <summary>
        /// get/set - Who the mail is being bcc'ed to.
        /// </summary>
        [TraceSetting("BCC", typeof(Fosol.Common.Parsers.Converters.FormatConverter))]
        public Format BCC
        {
            get { return _BCC; }
            set { _BCC = value; }
        }

        /// <summary>
        /// get/set - The mail subject.
        /// </summary>
        [TraceSetting("Subject", typeof(Fosol.Common.Parsers.Converters.FormatConverter))]
        [Required(AllowEmptyStrings = false)]
        public Format Subject
        {
            get { return _Subject; }
            set { _Subject = value; }
        }

        /// <summary>
        /// get/set - The mail body formatter.
        /// </summary>
        [TraceSetting("Body", typeof(Fosol.Common.Parsers.Converters.FormatConverter))]
        [Required(AllowEmptyStrings = false)]
        public Format Body
        {
            get { return _Body; }
            set { _Body = value; }
        }

        /// <summary>
        /// get/set - Controls whether the Smtp client will send a mail message in Html format.
        /// </summary>
        [DefaultValue(false)]
        [TraceSetting("IsBodyHtml")]
        public bool IsBodyHtml
        {
            get { return _IsBodyHtml; }
            set { _IsBodyHtml = value; }
        }

        /// <summary>
        /// get/set - The priority of the mail.
        /// </summary>
        [DefaultValue(MailPriority.Normal)]
        [TraceSetting("Priority", typeof(EnumConverter), typeof(MailPriority))]
        public MailPriority Priority
        {
            get { return _Priority; }
            set { _Priority = value; }
        }
        #endregion

        #region Constructors
        #endregion
        #region Methods
        /// <summary>
        /// Sends a mail message with the Smtp client.
        /// </summary>
        /// <param name="trace">TraceEvent object containing information sent to the listener.</param>
        protected override void OnWrite(TraceEvent trace)
        {
            SendMessage(CreateMessage(trace));
        }

        /// <summary>
        /// Creates a MailMessage based on the TraceEvent information.
        /// </summary>
        /// <param name="trace">TraceEvent object.</param>
        /// <returns>A new MailMessage object.</returns>
        private MailMessage CreateMessage(TraceEvent trace)
        {
            var from = new MailAddress(this.From.Render(trace));
            var to = new MailAddress(this.To.Render(trace));
            var mail = new MailMessage(from, to);

            var cc = this.CC.Render(trace);
            if (!string.IsNullOrEmpty(cc))
                mail.CC.Add(new MailAddress(cc));

            var bcc = this.BCC.Render(trace);
            if (!string.IsNullOrEmpty(bcc))
                mail.Bcc.Add(new MailAddress(bcc));

            mail.Priority = this.Priority;
            mail.Subject = this.Subject.Render(trace);
            mail.SubjectEncoding = this.Encoding;
            mail.IsBodyHtml = this.IsBodyHtml;
            mail.Body = this.Body.Render(trace);
            mail.BodyEncoding = this.Encoding;

            return mail;
        }

        /// <summary>
        /// Sends the MailMessage with the SmtpClient.
        /// </summary>
        /// <param name="message">MailMessage object to send with SmtpClient.</param>
        private void SendMessage(MailMessage message)
        {
            using (var client = new SmtpClient(this.SmtpHost, this.SmtpPort))
            {
                client.EnableSsl = this.EnableSsl;
                client.Timeout = this.Timeout;

                if (this.SmtpAuthentication == SmtpAuthenticationMode.Ntlm)
                    client.Credentials = CredentialCache.DefaultNetworkCredentials;
                else if (this.SmtpAuthentication == SmtpAuthenticationMode.Basic)
                    client.Credentials = new NetworkCredential(this.UserName, this.Password);

                client.Send(message);
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}

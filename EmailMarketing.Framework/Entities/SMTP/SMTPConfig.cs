using EmailMarketing.Data;
using System;

namespace EmailMarketing.Framework.Entities.SMTP
{
    public class SMTPConfig : IAuditableEntity<Guid>
    {
        public Guid UserId { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool EnableSSL { get; set; }
    }
}

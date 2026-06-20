using EmailMarketing.Shared.Infrastructure.Data;

namespace EmailMarketing.Shared.Infrastructure.Data.Entities.Notifications;

public class SMTPConfig : IAuditableEntity<Guid>
{
    public Guid UserId { get; set; }
    public string Server { get; set; } = string.Empty;
    public int Port { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public string SenderEmail { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool EnableSSL { get; set; }
}

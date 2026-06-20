using Microsoft.AspNetCore.Identity;

namespace EmailMarketing.Shared.Infrastructure;

public class ApplicationUserRole : IdentityUserRole<Guid>
{
    public virtual ApplicationUser? User { get; set; }
    public virtual ApplicationRole? Role { get; set; }
}


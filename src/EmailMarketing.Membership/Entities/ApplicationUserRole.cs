using Microsoft.AspNetCore.Identity;
using System;

namespace EmailMarketing.Membership.Entities
{
    public class ApplicationUserRole
        : IdentityUserRole<Guid>
    {
        public ApplicationUser User { get; set; }
        public ApplicationRole Role { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System;

namespace EmailMarketing.Membership.Entities
{
    public class ApplicationRoleClaim
        : IdentityRoleClaim<Guid>
    {
        public ApplicationRoleClaim()
            : base()
        {

        }
    }
}

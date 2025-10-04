using Microsoft.AspNetCore.Identity;
using System;

namespace EmailMarketing.Membership.Entities
{
    public class ApplicationUserClaim
        : IdentityUserClaim<Guid>
    {
        public ApplicationUserClaim()
            : base()
        {

        }
    }
}

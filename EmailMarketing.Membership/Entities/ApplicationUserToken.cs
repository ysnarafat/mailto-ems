using Microsoft.AspNetCore.Identity;
using System;

namespace EmailMarketing.Membership.Entities
{
    public class ApplicationUserToken
        : IdentityUserToken<Guid>
    {
        public ApplicationUserToken()
            : base()
        {

        }
    }
}

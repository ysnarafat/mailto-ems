using Microsoft.AspNetCore.Identity;
using System;

namespace EmailMarketing.Membership.Entities
{
    public class ApplicationUserLogin
        : IdentityUserLogin<Guid>
    {
        public ApplicationUserLogin()
            : base()
        {

        }
    }
}

using EmailMarketing.Shared.Infrastructure.Identity.Enums;
using Microsoft.AspNetCore.Identity;

namespace EmailMarketing.Shared.Infrastructure;

public class ApplicationRole : IdentityRole<Guid>
{
    public EnumApplicationRoleStatus Status { get; set; }

    public Guid? CreatedBy { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; }
    public IList<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();

    public ApplicationRole()
        : base()
    {
        IsActive = true;
        IsDeleted = false;
        UserRoles = new List<ApplicationUserRole>();
    }

    public ApplicationRole(string roleName)
        : base(roleName)
    {
        IsActive = true;
        IsDeleted = false;
        UserRoles = new List<ApplicationUserRole>();
    }
}


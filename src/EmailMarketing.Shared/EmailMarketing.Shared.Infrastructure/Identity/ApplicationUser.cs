using EmailMarketing.Shared.Infrastructure.Identity.Enums;
using Microsoft.AspNetCore.Identity;

namespace EmailMarketing.Shared.Infrastructure;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FullName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string ImageUrl { get; set; } = string.Empty;

    public string LastPassword { get; set; } = string.Empty;
    public DateTime? LastPassChangeDate { get; set; }
    public int PasswordChangedCount { get; set; }
    public EnumApplicationUserStatus Status { get; set; }

    public Guid? CreatedBy { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; }
    public bool IsBlocked { get; set; }
    public IList<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();

    public ApplicationUser()
        : base()
    {
        IsActive = true;
        IsDeleted = false;
        IsBlocked = false;
        UserRoles = new List<ApplicationUserRole>();
    }

    public ApplicationUser(string userName)
        : base(userName)
    {
        IsActive = true;
        IsDeleted = false;
        IsBlocked = false;
        UserRoles = new List<ApplicationUserRole>();
    }

    public ApplicationUser(string userName, string phoneNumber, string email)
        : this(userName)
    {
        Email = email;
        PhoneNumber = phoneNumber;
    }

    public ApplicationUser(string userName, string fullName, string phoneNumber, string email)
        : this(userName, phoneNumber, email)
    {
        FullName = fullName;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not ApplicationUser user)
            return false;

        return Id == user.Id;
    }

    public override int GetHashCode()
    {
        var hashCode = -582740416;
        hashCode = hashCode * -1521134295 + EqualityComparer<Guid>.Default.GetHashCode(Id);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(UserName);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Email);
        return hashCode;
    }
}


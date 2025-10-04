using EmailMarketing.Membership.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace EmailMarketing.Membership.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ImageUrl { get; set; }

        public string LastPassword { get; set; }
        public DateTime? LastPassChangeDate { get; set; }
        public int PasswordChangedCount { get; set; }
        public EnumApplicationUserStatus Status { get; set; }

        public Guid? CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsBlocked { get; set; }
        public IList<ApplicationUserRole> UserRoles { get; set; }

        public ApplicationUser()
                    : base()
        {
            this.IsActive = true;
            this.IsDeleted = false;
            this.IsBlocked = false;
            this.UserRoles = new List<ApplicationUserRole>();
        }

        public ApplicationUser(string userName)
            : base(userName)
        {
            this.IsActive = true;
            this.IsDeleted = false;
            this.IsBlocked = false;
            this.UserRoles = new List<ApplicationUserRole>();
        }

        public ApplicationUser(string userName, string phoneNumber, string email)
            : this(userName)
        {
            this.Email = email;
            this.PhoneNumber = phoneNumber;
        }

        public ApplicationUser(string userName, string fullName, string phoneNumber, string email)
            : this(userName, phoneNumber, email)
        {
            this.FullName = fullName;
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
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
}

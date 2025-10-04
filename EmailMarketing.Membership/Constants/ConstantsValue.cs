namespace EmailMarketing.Membership.Constants
{
    public static class ConstantsValue
    {
        public static UserRoleName UserRoleName => new UserRoleName();
    }

    public class UserRoleName
    {
        public string SuperAdmin => "SuperAdmin";
        public string Admin => "Admin";
        public string Member => "Member";
    }

    public class ConstantsUserRoleName
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string SuperAdminOrAdmin = "SuperAdmin,Admin";
        public const string SuperAdminOrMember = "SuperAdmin,Member";
        public const string Admin = "Admin";
        public const string Member = "Member";
    }
}

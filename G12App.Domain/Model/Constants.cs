namespace G12App.Domain.Model
{
    public static class CustomClaims
    {
        public const string MemberId = nameof(MemberId);

        public const string MinistryLevel = nameof(MinistryLevel);

        public const string LeadershipId = nameof(LeadershipId);
    }

    public static class Roles
    {
        public const string AppUser = nameof(AppUser);

        public const string AppAdmin = nameof(AppAdmin);

        public const string AppSuperAdmin = nameof(AppSuperAdmin);
    }
}

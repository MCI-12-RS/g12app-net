namespace G12App.Domain.Model
{
    public static class CustomClaims
    {
        public const string MemberId = nameof(MemberId);

        public const string MinistryLevel = nameof(MinistryLevel);

        public const string LeadershipId = nameof(LeadershipId);

        public const string G12LeadershipName = nameof(G12LeadershipName);

        public const string SpouseId = nameof(SpouseId);

        public static string G12LeadershipId = nameof(G12LeadershipId);

        public static string G12LeadershipSpouseId = nameof(G12LeadershipSpouseId);
    }

    public static class Roles
    {
        public const string AppUser = nameof(AppUser);

        public const string AppAdmin = nameof(AppAdmin);

        public const string AppSuperAdmin = nameof(AppSuperAdmin);
    }
}

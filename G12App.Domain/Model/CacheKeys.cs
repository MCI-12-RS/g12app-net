using G12App.Domain.Caching;

namespace G12App.Domain.Model
{
    public static class CacheKeys
    {
        // Member Count
        public static readonly CacheSettings Member_CreatedCurrentWeek = new CacheSettings(nameof(Member_CreatedCurrentWeek));
        public static readonly CacheSettings Member_CountActiveMembers = new CacheSettings(nameof(Member_CountActiveMembers));
        public static readonly CacheSettings Member_CountWithoutLeadership = new CacheSettings(nameof(Member_CountWithoutLeadership));
        public static readonly CacheSettings Member_CountWithNoCellLink = new CacheSettings(nameof(Member_CountWithNoCellLink));
        public static readonly CacheSettings Member_CountNonBaptized = new CacheSettings(nameof(Member_CountNonBaptized));
        public static readonly CacheSettings Member_CountIdle = new CacheSettings(nameof(Member_CountIdle));
        public static readonly CacheSettings Member_CountCreatedCurrentWeek = new CacheSettings(nameof(Member_CountCreatedCurrentWeek));

        // Cell Count
        public static readonly CacheSettings Cell_CountTotalCells = new CacheSettings(nameof(Cell_CountTotalCells));
        public static readonly CacheSettings Cell_CountPendingReports = new CacheSettings(nameof(Cell_CountPendingReports));
        public static readonly CacheSettings Cell_CountWeekVisitors = new CacheSettings(nameof(Cell_CountWeekVisitors));
        public static readonly CacheSettings Cell_CountWeekReports = new CacheSettings(nameof(Cell_CountWeekReports));
        public static readonly CacheSettings Cell_CountWeekNewCells = new CacheSettings(nameof(Cell_CountWeekNewCells));
        public static readonly CacheSettings Cell_CountReportSkipped = new CacheSettings(nameof(Cell_CountReportSkipped));
        public static readonly CacheSettings Cell_CountReportedCompleted = new CacheSettings(nameof(Cell_CountReportedCompleted));
    }
}

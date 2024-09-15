namespace G12App.Domain.DTOs
{
    public class DashboardDto
    {
        public int TotalMembers { get; set; }

        public int TotalCells { get; set; }

        public int WeekReports { get; set; }

        public int CellsPendingReports { get; set; }

        public int WeekVisitors { get; set; }

        public int WeekNewMembers { get; set; }

        public int TotalIdleMembers { get; set; }

        public int TotalNonBaptizedMembers { get; set; }

        public int TotalCellReportedCompleted { get; set; }

        public int TotalCellReportedSkipped { get; set; }

        public int WeekNewCells { get; set; }

        public int TotalMembersWithoutLeadership { get; set; }
        public int TotalMembersWithNoCellLink { get; set; }
    }
}

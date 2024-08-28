namespace G12App.Domain.DTOs
{
    public class DashboardDto
    {
        public int TotalMembers { get; set; }

        public int TotalCells { get; set; }

        public int WeekReports { get; set; }

        public int CellsPendingReports { get; set; }

        public int WeekVisitors { get; set; }
    }
}

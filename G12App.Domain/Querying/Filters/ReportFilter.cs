using G12App.Domain.Model.Enum;
using System;

namespace G12App.Domain.Querying.Filters
{
    public class ReportFilter : BaseQueryFilter
    {
        public string Subject { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public CellType? CellType { get; set; }
        public int LeadershipId { get; set; }
        public int HostId { get; set; }
        public int TimothyId { get; set; }
        public int? CellId { get; set; }

        public bool? WeekCellSkipped { get; set; }
    }
}

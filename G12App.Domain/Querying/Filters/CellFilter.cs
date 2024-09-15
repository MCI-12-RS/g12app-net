using G12App.Domain.Model.Enum;
using System;

namespace G12App.Domain.Querying.Filters
{
    public class CellFilter : BaseQueryFilter
    {
        public int LeadId { get; set; }
        public string Lead { get; set; }
        public string Leadership { get; set; }
        public int HostId { get; set; }
        public string Host { get; set; }
        public int TimothyId { get; set; }
        public string Timothy { get; set; }
        public DayOfWeek? WeekDay { get; set; }
        public string HomeAddress_Neighborhood { get; set; }
        public string HomeAddress_City { get; set; }
        public string HomeAddress_State { get; set; }
        public string HomeAddress_Street { get; set; }
        public CellType? CellType { get; set; }
        public int? LeadIdAllLevels { get; set; }
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateUntil { get; set; }
        public CellReportStatus? ReportStatus { get; set; }
    }
}

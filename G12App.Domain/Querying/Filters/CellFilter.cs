using G12App.Domain.Model.Enum;
using System;

namespace G12App.Domain.Querying.Filters
{
    public class CellFilter : BaseQueryFilter
    {
        public int LeadId { get; set; }
        public int HostId { get; set;}
        public int TimothyId { get; set; }
        public DayOfWeek WeekDay { get; set; }
        public int PlaceId { get; set; }
        public CellType CellType { get; set; }
    }
}

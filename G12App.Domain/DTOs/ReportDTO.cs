using G12App.Domain.Model.Enum;
using System;

namespace G12App.Domain.DTOs
{
    public class ReportDTO
    {
        public int? ReportId { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public TimeOnly Time { get; set; }
        public int Visitors { get; set; }
        public int Frequenters { get; set; }
        public int Members { get; set; }
        public int CellId { get; set; }
        public int LeadershipId { get; set; }
        public int HostId { get; set; }
        public int? TimothyId { get; set; }
        public CellType? CellType { get; set; }
    }
}

using G12App.Domain.Model.Base;
using G12App.Domain.Model.Enum;
using System;
using System.Collections.Generic;

namespace G12App.Domain.Model
{
    public class Report : BaseEntity
    {
        public int ReportId { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public int Visitors { get; set; }
        public ICollection<Member> Members { get; set; }
        public int CellId { get; set; }
        public int LeadershipId { get; set; }
        public int HostId { get; set; }
        public int TimothyId { get; set; }
        public CellType? CellType { get; set; }
    }
}

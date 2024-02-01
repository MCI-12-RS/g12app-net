using G12App.Domain.Model;
using G12App.Domain.Model.Enum;
using System;
using System.Collections.Generic;

namespace G12App.Domain.DTOs
{
    public class ReportDTO
    {
        public int? Id { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public string Visitors { get; set; }
        public List<int> FrequentersIds { get; set; }
        public List<Member> Members { get; set; }
        public int CellId { get; set; }
        public int LeadershipId { get; set; }
        public int HostId { get; set; }
        public int? TimothyId { get; set; }
        public CellType? CellType { get; set; }
    }
}

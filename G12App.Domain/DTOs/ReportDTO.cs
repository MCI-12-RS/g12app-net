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
        public List<string> Visitors { get; set; }
        public List<int> Frequenters { get; set; }
        public List<MemberDto> Members { get; set; }
        public int CellId { get; set; }
        public int LeadershipId { get; set; }
        public int HostId { get; set; }
        public int TimothyId { get; set; }
        public CellType? CellType { get; set; }
        public decimal Contribution { get; set; }
        public int NumVisitors { get; set; }
        public int NumMembers { get; set; }
        public string Notes { get; set; }
        public string DateOnly => Date.ToString("dd/MM/yyyy");
    }
}

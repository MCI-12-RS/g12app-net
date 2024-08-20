using G12App.Domain.Model.Enum;
using System;

namespace G12App.Domain.DTOs
{
    public class CellDto
    {
        public int? Id { get; set; }
        public int LeadId { get; set; }
        public MemberDto Lead { get; set; }
        public int HostId { get; set; }
        public MemberDto Host { get; set; }
        public int TimothyId { get; set; }
        public MemberDto Timothy { get; set; }
        public DayOfWeek WeekDay { get; set; }
        public TimeOnly Time { get; set; }
        public int? PlaceId { get; set; }
        public AddressDto Place { get; set; }
        public CellType CellType { get; set; }
        public bool ReportMandatory { get; set; }
    }
}

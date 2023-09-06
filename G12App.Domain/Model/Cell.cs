using G12App.Domain.Model.Base;
using G12App.Domain.Model.Enum;
using System;

namespace G12App.Domain.Model
{
    public class Cell : BaseEntity
    {
        public int Id { get; set; }
        public Member Lead { get; set; }
        public int LeadId { get; set; }
        public Member Host { get; set; }
        public int HostId { get; set; }
        public Member Timothy { get; set; }
        public int TimothyId { get; set; }
        public DayOfWeek WeekDay { get; set; }
        public Address Place { get; set; }
        public CellType CellType { get; set; }
        public bool ReportMandatory { get; set; }
    }
}

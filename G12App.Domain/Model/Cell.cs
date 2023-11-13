using G12App.Domain.Model.Base;
using G12App.Domain.Model.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace G12App.Domain.Model
{
    public class Cell : BaseEntity
    {
        public Member Lead { get; set; }
        public int LeadId { get; set; }
        public Member Host { get; set; }
        public int HostId { get; set; }
        public Member Timothy { get; set; }
        public int TimothyId { get; set; }
        public DayOfWeek WeekDay { get; set; }
        public string Time { get; set; }
        public Address Place { get; set; }
        public int? PlaceId { get; set; }
        public CellType CellType { get; set; }
        public bool ReportMandatory { get; set; }
    }
}

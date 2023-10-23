using G12App.Domain.Model.Enum;
using G12App.Domain.Model;
using System;
using Microsoft.AspNetCore.Mvc;

namespace G12App.Domain.DTOs
{
    public class CellDTO
    {
        public int? Id { get; set; }
        public int LeadId { get; set; }
        public Member Lead { get; set; }
        public int HostId { get; set; }
        public Member Host { get; set; }
        public int TimothyId { get; set; }
        public Member Timothy { get; set; }
        public DayOfWeek WeekDay { get; set; }
        public TimeOnly Time { get; set; }
        public int? PlaceId { get; set; }
        public Address? Place { get; set; }
        public CellType CellType { get; set; }
        public bool ReportMandatory { get; set; }
    }
}

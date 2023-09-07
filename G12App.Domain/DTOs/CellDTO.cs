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
        public int HostId { get; set; }
        public int TimothyId { get; set; }
        public DayOfWeek WeekDay { get; set; }
        public AddressDto Place { get; set; }
        public CellType CellType { get; set; }
        public bool ReportMandatory { get; set; }
    }
}

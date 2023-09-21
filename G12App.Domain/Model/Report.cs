using G12App.Domain.Model.Base;
using System;

namespace G12App.Domain.Model
{
    public class Report : BaseEntity
    {
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public int Visitors { get; set; }
        public int Frequenters { get; set; }
        public int Members { get; set; }
        public Cell Cell { get; set; }
        public int LeadershipId { get; set; }
        public int HostId { get; set; }
        public int TimothyId { get; set; }
    }
}

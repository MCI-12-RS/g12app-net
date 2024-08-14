using G12App.Domain.Model.Base;
using G12App.Domain.Model.Enum;
using System;
using System.Collections.Generic;

namespace G12App.Domain.Model
{
    public class Report : BaseEntity
    {
        public string Subject { get; set; }

        public DateTime Date { get; set; }

        public string Visitors { get; set; }

        public ICollection<Member> Members { get; set; }

        public int CellId { get; set; }

        public Member Lead { get; set; }

        public int LeadershipId { get; set; }

        public Member Host { get; set; }

        public int HostId { get; set; }

        public Member Timothy { get; set; }

        public int TimothyId { get; set; }

        public CellType? CellType { get; set; }
    }
}

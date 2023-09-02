using System;

namespace G12App.Domain.Model
{
    public class Report
    {
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public int Visitors { get; set; }
        public int Frequenters { get; set; }
        public int Members { get; set; }
        public Cell Cell { get; set; }
    }
}

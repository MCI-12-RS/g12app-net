using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace G12App.Domain.Model
{
    [NotMapped]
    public class CellReports
    {
        public Cell Cell { get; set; }
        public ICollection<Report> Reports { get; set; }
    }
}

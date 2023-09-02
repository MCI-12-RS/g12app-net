using G12App.Domain.Model.Enum;

namespace G12App.Domain.Model
{
    public class Cell
    {
        public int Id { get; set; }
        public Member Lead { get; set; }
        public Member Host { get; set; }
        public Member Timothy { get; set; }
        public WeekDay WeekDay { get; set; }
        public Address Place { get; set; }
        public CellType CellType { get; set; }
        public bool ReportMandatory { get; set; }
    }
}

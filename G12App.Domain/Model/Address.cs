using G12App.Domain.Model.Base;

namespace G12App.Domain.Model
{
    public class Address : BaseEntity
    {
        public string Street { get; set; }
        public string Number { get; set; }
        public string Notes { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    }
}

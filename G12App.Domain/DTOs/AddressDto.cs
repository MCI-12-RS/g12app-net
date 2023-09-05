using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace G12App.Domain.DTOs
{
    public class AddressDto
    {
        public int? Id { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Notes { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        
    }
}
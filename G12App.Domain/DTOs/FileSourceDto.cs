using System.Collections.Generic;

namespace G12App.Domain.DTOs
{
    public class FileSourceDto
    {
        public List<string> Columns { get; set; }

        public List<List<object>> Data { get; set; }
    }
}

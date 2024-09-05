using System.IO;

namespace G12App.Domain.DTOs
{
    public class FileDto
    {
        public Stream Stream { get; set; }

        public string ContentType { get; set; }
    }
}

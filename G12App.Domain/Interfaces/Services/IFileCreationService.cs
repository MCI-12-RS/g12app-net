using G12App.Domain.DTOs;
using G12App.Domain.Messages;

namespace G12App.Domain.Interfaces.Services
{
    public interface IFileCreationService
    {
        string ContentType { get; }

        ServiceResult<FileDto> CreateFile(FileSourceDto fileSource);
    }
}

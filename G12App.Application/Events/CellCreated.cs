using G12App.Domain.DTOs;

namespace G12App.Application.Events
{
    public class CellCreated : CellChanged
    {
        public CellCreated(CellDto cell)
            : base(null, cell)
        {
        }
    }
}

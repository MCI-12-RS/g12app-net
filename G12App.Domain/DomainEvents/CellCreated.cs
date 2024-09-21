using G12App.Domain.DTOs;

namespace G12App.Domain.DomainEvents
{
    public class CellCreated : CellChanged
    {
        public CellCreated(CellDto cell)
            : base(null, cell)
        {
        }
    }
}

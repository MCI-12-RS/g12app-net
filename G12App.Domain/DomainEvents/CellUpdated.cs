using G12App.Domain.DTOs;

namespace G12App.Domain.DomainEvents
{
    public class CellUpdated : CellChanged
    {
        public CellUpdated(CellDto before, CellDto after)
            : base(before, after)
        {
        }
    }
}

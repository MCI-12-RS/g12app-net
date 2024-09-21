using G12App.Domain.DTOs;

namespace G12App.Domain.DomainEvents
{
    public class CellDeleted : CellChanged
    {
        public CellDeleted(CellDto cell)
            : base(cell, null)
        {
        }
    }
}

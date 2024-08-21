using G12App.Domain.DTOs;

namespace G12App.Application.Events
{
    public class CellDeleted : CellChanged
    {
        public CellDeleted(CellDto cell)
            : base(cell, null)
        {
        }
    }
}

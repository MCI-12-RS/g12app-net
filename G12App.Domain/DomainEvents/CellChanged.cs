using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Events;

namespace G12App.Domain.DomainEvents
{
    public abstract class CellChanged : BaseEvent, IEvent
    {
        public CellDto Before { get; }
        public CellDto After { get; }

        public override object EntityId => Before?.Id ?? After?.Id;

        public CellChanged(CellDto before, CellDto after)
            => (Before, After) = (before, after);

    }
}

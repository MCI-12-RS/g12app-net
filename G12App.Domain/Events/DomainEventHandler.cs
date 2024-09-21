using System.Threading.Tasks;

namespace G12App.Domain.Events
{

    public delegate Task DomainEventHandler(DomainEventArgs args);
}

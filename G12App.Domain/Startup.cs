using G12App.Domain.Mappings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace G12App.Domain
{
    public static class Startup
    {
        public static IServiceCollection RegisterDomain(this IServiceCollection servicesCollection)
        {
            servicesCollection.AddAutoMapper(typeof(DomainToDTOMappingProfile));
            return servicesCollection;
        }

    }
}

using G12App.Domain.DomainEvents;
using G12App.Domain.Interfaces.EventHandlers;
using G12App.Domain.Interfaces.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G12App.Domain.Events
{
    public class EventHandlerRegistry : IEventHandlerRegistry
    {
        public IReadOnlyDictionary<Type, Func<IServiceProvider, IEvent, Task>> Handlers => _handlers;

        readonly IReadOnlyDictionary<Type, Func<IServiceProvider, IEvent, Task>> _handlers =
            new Dictionary<Type, Func<IServiceProvider, IEvent, Task>>
            {
                // Member
                { typeof(MemberChanged), 
                    (provider, ev) => provider.GetRequiredService<IMemberChangedHandler>().Handle(ev as MemberChanged) },
                { typeof(MemberUpdated), 
                    (provider, ev) => provider.GetRequiredService<IMemberChangedHandler>().Handle(ev as MemberChanged) },
                { typeof(MemberCreated), 
                    (provider, ev) => provider.GetRequiredService<IMemberChangedHandler>().Handle(ev as MemberChanged) },
                { typeof(MemberDeleted), 
                    (provider, ev) => provider.GetRequiredService<IMemberChangedHandler>().Handle(ev as MemberChanged) },
                { typeof(MemberDataConsolidationRequested), 
                    (provider, ev) => provider.GetRequiredService<IMemberChangedHandler>().Handle(ev as MemberChanged) },


            };
    }
}

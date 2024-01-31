using Ardalis.SharedKernel;
using MediatR;
using Request.Module.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Module.Infrastructure.Persistence
{
    public class DomainEventDispatcherCustom : MediatRDomainEventDispatcher, IDomainEventDispatcherCustom
    {
        private readonly IMediator _mediator;

        public DomainEventDispatcherCustom(IMediator mediator) : base(mediator)
        {
            _mediator = mediator;
        }


        public async Task DispatchAndClearEvents(IEnumerable<EntityBaseCustom<Guid>> entitiesWithEvents)
        {
            foreach (EntityBaseCustom<Guid> entityWithEvents in entitiesWithEvents)
            {
                DomainEventBase[] events = entityWithEvents.DomainEvents.ToArray();
                entityWithEvents.ClearDomainEvents();
                DomainEventBase[] events2 = events;
                foreach (DomainEventBase notification in events2)
                {
                    await _mediator.Publish(notification).ConfigureAwait(continueOnCapturedContext: false);
                }
            }
        }
    }

    public interface IDomainEventDispatcherCustom : IDomainEventDispatcher
    {
        Task DispatchAndClearEvents(IEnumerable<EntityBaseCustom<Guid>> entitiesWithEvents);
    }
}

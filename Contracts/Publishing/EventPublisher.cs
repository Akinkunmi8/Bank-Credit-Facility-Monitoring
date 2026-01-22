namespace Contracts.Publishing
{
    using Contracts.Events;
    using Microsoft.Extensions.DependencyInjection;

    public class EventPublisher : IEventPublisher
    {
        private readonly IServiceProvider _serviceProvider;

        public EventPublisher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task PublishAsync(IDomainEvent domainEvent)
        {
            // Resolve all handlers for this event type
            var handlerType = typeof(IEventHandler<>).MakeGenericType(domainEvent.GetType());
            var handlers = _serviceProvider.GetServices(handlerType);

            foreach (dynamic handler in handlers)
            {
                await handler.HandleAsync((dynamic)domainEvent);
            }
        }
    }
}

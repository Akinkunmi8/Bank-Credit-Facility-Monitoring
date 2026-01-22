using Contracts.Events;
namespace Contracts.Publishing
{
    public interface IEventPublisher
    {
        Task PublishAsync(IDomainEvent domainEvent);
    }

}

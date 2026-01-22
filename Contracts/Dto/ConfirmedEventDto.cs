using Contracts.Events;

namespace Contracts.Dto
{
    public record ConfirmedEvent(Guid AppId) : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }

    public record RejectedEvent(Guid AppId) : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }

    public record ApprovedEvents(Guid AppId) : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }

    public record TATEvent(Guid AppId) : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
    public record OnbordEvent(Guid UserId) : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }

    //public class EventAction : IDomainEvent
    //{
    //    public Guid UserId { get; init; }
    //    public Guid AppId { get; init; }
    //    //public string IpAddress { get; init; }

    //    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    //}


}

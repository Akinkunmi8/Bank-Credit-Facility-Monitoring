namespace Contracts.Events
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }

}

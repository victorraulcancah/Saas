namespace Backend.SharedKernel.Common.Events;

public interface IDomainEvent
{
    Guid Id { get; }
    DateTime OccurredAtUtc { get; }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace PomoziAuctions.SharedKernel;

public abstract class EntityBase : EntityBase<int>
{
}

public abstract class EntityBase<TKey>
	where TKey : IEquatable<TKey>
{
	private readonly List<DomainEventBase> _domainEvents = new();

	public TKey Id { get; set; }

	[NotMapped]
	public IEnumerable<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

	protected void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);

	internal void ClearDomainEvents() => _domainEvents.Clear();
}

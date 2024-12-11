using Ardalis.Specification;

namespace PomoziAuctions.SharedKernel.Interfaces;

public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
}

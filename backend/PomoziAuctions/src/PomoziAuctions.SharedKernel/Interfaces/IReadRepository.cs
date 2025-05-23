﻿using Ardalis.Specification;

namespace PomoziAuctions.SharedKernel.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
}

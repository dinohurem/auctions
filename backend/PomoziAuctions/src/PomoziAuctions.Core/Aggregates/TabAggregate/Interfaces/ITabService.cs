using Ardalis.Result;
using PomoziAuctions.Core.Aggregates.TabAggregate.Models;

namespace PomoziAuctions.Core.Aggregates.TabAggregate.Interfaces;
public interface ITabService
{
  Task<Result<TabDto>> Update(TabDto tab);

  Task<Result<List<TabDto>>> Get();
}

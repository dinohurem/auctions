using PomoziAuctions.SharedKernel;
using PomoziAuctions.SharedKernel.Interfaces;

namespace PomoziAuctions.Core.Aggregates.TabAggregate.Models;

public class Tab : EntityBase, IAggregateRoot
{
  public string Name { get; set; }

  public string Route { get; set; }

  public bool IsHidden { get; set; }
}

using PomoziAuctions.SharedKernel;
using PomoziAuctions.SharedKernel.Interfaces;

namespace PomoziAuctions.Core.Aggregates.BlobAggregate.Models;

public class Blob : EntityBase, IAggregateRoot, ISoftDelete
{
  public string Name { get; set; }

  public string NormalizedName { get; set; }

  public bool Deleted { get; set; }
}

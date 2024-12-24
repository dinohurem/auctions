using PomoziAuctions.SharedKernel;
using PomoziAuctions.SharedKernel.Interfaces;

namespace PomoziAuctions.Core;
public abstract class FullAuditCompanyAggregateRoot : EntityBase, ISoftDelete, IAggregateRoot
{
  // Any property can be added here.

  public bool Deleted { get; set; }
}

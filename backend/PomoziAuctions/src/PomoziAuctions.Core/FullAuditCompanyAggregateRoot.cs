using PomoziAuctions.Core.Aggregates.CompanyAggregate.Interfaces;
using PomoziAuctions.SharedKernel;
using PomoziAuctions.SharedKernel.Interfaces;

namespace PomoziAuctions.Core;
public abstract class FullAuditCompanyAggregateRoot : EntityBase, ICompanyKey, ISoftDelete, IAggregateRoot
{
  public int? CompanyId { get; set; }

  public bool Deleted { get; set; }
}

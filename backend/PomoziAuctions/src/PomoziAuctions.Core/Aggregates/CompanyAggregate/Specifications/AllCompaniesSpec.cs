using Ardalis.Specification;
using PomoziAuctions.Core.Aggregates.CompanyAggregate.Models;

namespace PomoziAuctions.Core.Aggregates.CompanyAggregate.Specifications;

public class AllCompaniesSpec : Specification<Company>
{
  public AllCompaniesSpec()
  {
    Query
      .Include(x => x.Blob)
      .Where(x => !x.Deleted).IgnoreQueryFilters();
  }
}

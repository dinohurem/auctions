using Ardalis.Specification;
using PomoziAuctions.Core.Aggregates.CompanyAggregate.Models;

namespace PomoziAuctions.Core.Aggregates.CompanyAggregate.Specifications;

public class SearchCompanyByIdSpec : Specification<Company>, ISingleResultSpecification<Company>
{
	public SearchCompanyByIdSpec(int id)
	{
		Query
	  .Include(x => x.Blob)
	  .Where(item => item.Id == id && (item.PendingApproval == false || item.PendingApproval == null) && !item.Deleted);
	}
}

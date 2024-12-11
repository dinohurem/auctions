using Ardalis.Specification;
using PomoziAuctions.Core.Aggregates.CompanyAggregate.Models;
using PomoziAuctions.SharedKernel.Specifications;

namespace CompanyFair.Core.Aggregates.CompanyAggregate.Specifications;
public class AllCompaniesFilteredPaginatedSpec : PaginatedSpecification<Company>
{
  public AllCompaniesFilteredPaginatedSpec(FilterCompanyDto filter)
      : base(filter.PageNumber, filter.PageSize)
  {

    // Include related entities
    Query.Include(company => company.Blob);

    Query.Where(x => !x.Deleted);

    // Apply location filter
    if (!string.IsNullOrWhiteSpace(filter.Location))
    {
      Query.Where(company => company.City.ToLower().Contains(filter.Location.ToLower()));
    }

    // Apply location filter
    if (!string.IsNullOrWhiteSpace(filter.Keyword))
    {
      Query.Where(company => company.Name.ToLower().Contains(filter.Keyword.ToLower()));
    }

    // Apply industry filter
    if (!string.IsNullOrWhiteSpace(filter.Industry))
    {
      Query.Where(company => company.Industry.ToLower().Contains(filter.Industry.ToLower()));
    }

    // Order by creation date (descending)
    Query.OrderBy(company => company.Name);

    // Ignore global query filters
    Query.IgnoreQueryFilters();

  }
}

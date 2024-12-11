using Ardalis.Specification;

namespace PomoziAuctions.SharedKernel.Specifications;

public class PaginatedSpecification<T> : Specification<T>
{
  public int PageSize { get; }
  public int PageNumber { get; }

  public PaginatedSpecification(int pageNumber, int pageSize)
  {
    PageNumber = pageNumber;
    PageSize = pageSize;
    ApplyPaging();
  }

  private void ApplyPaging()
  {
    if (PageSize > 0)
    {
      Query.Skip((PageNumber - 1) * PageSize)
           .Take(PageSize);
    }
  }
}

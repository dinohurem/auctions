namespace PomoziAuctions.SharedKernel.Specifications;

public class PaginatedResult<T>
{
  public List<T> Items { get; set; }
  public int TotalCount { get; set; }
  public int CurrentPage { get; set; }
  public int PageSize { get; set; }

  public PaginatedResult(List<T> items, int totalCount, int currentPage, int pageSize)
  {
    Items = items;
    TotalCount = totalCount;
    CurrentPage = currentPage;
    PageSize = pageSize;
  }
}

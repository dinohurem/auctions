namespace PomoziAuctions.Core.Aggregates.CompanyAggregate.Models;

public class FilterCompanyDto
{
  public int PageNumber { get; set; } = 1;
  public int PageSize { get; set; } = 10;
  public string? Keyword { get; set; }
  public string? Location { get; set; }
  public string? Industry { get; set; }
}

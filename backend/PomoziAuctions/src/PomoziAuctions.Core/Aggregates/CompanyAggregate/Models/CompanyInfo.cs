namespace PomoziAuctions.Core.Aggregates.CompanyAggregate.Models;

public class CompanyInfo
{
  public int? Id { get; set; }

  public string Name { get; set; }

  public string Type { get; set; }

  public Guid? PackageId { get; }
}


namespace PomoziAuctions.Core.Aggregates.BlobAggregate.Models;

public class BlobDto
{
  public int Id { get; set; }

  public string Name { get; set; }

  public string NormalizedName { get; set; }

  public int? CompanyId { get; set; }
}

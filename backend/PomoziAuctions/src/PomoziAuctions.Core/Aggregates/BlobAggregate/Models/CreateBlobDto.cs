namespace PomoziAuctions.Core.Aggregates.BlobAggregate.Models;
public class CreateBlobDto
{
  public string Uri { get; set; }

  public string NormalizedName { get; set; }

  public string FileName { get; set; }

  public string ContentType { get; set; }

  public Stream Content { get; set; }
}

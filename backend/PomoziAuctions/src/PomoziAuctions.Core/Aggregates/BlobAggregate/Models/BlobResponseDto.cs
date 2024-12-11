namespace PomoziAuctions.Core.Aggregates.BlobAggregate.Models;
public class BlobResponseDto
{
  public string Status { get; set; }
  public bool Error { get; set; }
  public CreateBlobDto Blob { get; set; }

  public BlobResponseDto()
  {
    Blob = new CreateBlobDto();
  }
}

using Ardalis.Result;
using PomoziAuctions.Core.Aggregates.BlobAggregate.Models;

namespace PomoziAuctions.Core.Aggregates.BlobAggregate.Interfaces;
public interface IBlobService
{
  Task<Result<BlobDto>> CreateAsync(CreateBlobDto blobDto);

  Task<Result<BlobDto>> GetAsync(int id);

  Task<Result<BlobDto>> DeleteAsync(int id);
}

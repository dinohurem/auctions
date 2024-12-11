using Ardalis.Result;
using AutoMapper;
using PomoziAuctions.Core.Aggregates.BlobAggregate.Interfaces;
using PomoziAuctions.Core.Aggregates.BlobAggregate.Models;
using PomoziAuctions.SharedKernel.Interfaces;

namespace PomoziAuctions.Core.Aggregates.BlobAggregate.Services;

public class BlobService : IBlobService
{
  private readonly IRepository<Blob> _repository;
  private readonly IMapper _mapper;

  public BlobService(IRepository<Blob> repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  public async Task<Result<BlobDto>> CreateAsync(CreateBlobDto blobDto)
  {
    var newBlob = _mapper.Map<Blob>(blobDto);
    var blob = await _repository.AddAsync(newBlob);
    return new Result<BlobDto>(_mapper.Map<Blob, BlobDto>(blob));
  }

  public async Task<Result<BlobDto>> GetAsync(int id)
  {
    var blob = await _repository.GetByIdAsync(id);
    if (blob == null)
    {
      return Result<BlobDto>.NotFound();
    }
    return new Result<BlobDto>(_mapper.Map<Blob, BlobDto>(blob));
  }

  public async Task<Result<BlobDto>> DeleteAsync(int id)
  {
    var blob = await _repository.GetByIdAsync(id);

    if (blob == null)
    {
      return Result<BlobDto>.NotFound();
    }

    blob.Deleted = true;
    await _repository.UpdateAsync(blob);
    return new Result<BlobDto>(_mapper.Map<Blob, BlobDto>(blob));
  }
}

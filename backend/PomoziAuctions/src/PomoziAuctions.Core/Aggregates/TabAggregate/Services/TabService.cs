using Ardalis.Result;
using AutoMapper;
using PomoziAuctions.Core.Aggregates.TabAggregate.Interfaces;
using PomoziAuctions.Core.Aggregates.TabAggregate.Models;
using PomoziAuctions.SharedKernel.Interfaces;

namespace PomoziAuctions.Core.Aggregates.TabAggregate.Services;

public class TabService : ITabService
{
  private readonly IRepository<Tab> _repository;
  private readonly IMapper _mapper;

  public TabService(IRepository<Tab> repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  public async Task<Result<TabDto>> Update(TabDto tabDto)
  {
    var tab = await _repository.GetByIdAsync(tabDto.Id);

    if (tab == null)
    {
      return Result<TabDto>.NotFound();
    }

    _mapper.Map(tabDto, tab);
    await _repository.UpdateAsync(tab);

    return new Result<TabDto>(_mapper.Map<TabDto>(tab));
  }

  public async Task<Result<List<TabDto>>> Get()
  {
    var tabs = await _repository.ListAsync();
    return new Result<List<TabDto>>(_mapper.Map<List<TabDto>>(tabs));
  }

}

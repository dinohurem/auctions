using PomoziAuctions.Core.Aggregates.TabAggregate.Interfaces;
using PomoziAuctions.Core.Aggregates.TabAggregate.Models;
using Microsoft.AspNetCore.Mvc;

namespace PomoziAuctions.Web.Api;

public class TabsController : BaseApiController
{

  private readonly ITabService _tabService;

  public TabsController(ITabService tabService)
  {
    _tabService = tabService;
  }

  [HttpGet]
  public async Task<IActionResult> GetAsync()
  {
    return MapResult(await _tabService.Get());
  }


  [HttpPut]
  public async Task<IActionResult> Update([FromBody] TabDto request)
  {
    return MapResult(await _tabService.Update(request));
  }

}

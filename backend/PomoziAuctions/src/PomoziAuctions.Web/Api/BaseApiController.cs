using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;

namespace PomoziAuctions.Web.Api;

/// <summary>
/// If your API controllers will use a consistent route convention and the [ApiController] attribute (they should)
/// then it's a good idea to define and use a common base controller class like this one.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public abstract class BaseApiController : Controller
{
  /// <summary>
  /// Maps provided result to <see cref="IActionResult"/>.
  /// </summary>
  /// <typeparam name="T">Result payload.</typeparam>
  /// <param name="result">Result.</param>
  /// <returns>Action result.</returns>
  public IActionResult MapResult<T>(Result<T> result)
  {
    switch (result.Status)
    {
      case ResultStatus.Ok:
        return Ok(result.Value);
      case ResultStatus.Error:
        return StatusCode(500);
      case ResultStatus.Forbidden:
        return Forbid();
      case ResultStatus.Unauthorized:
        return Unauthorized();
      case ResultStatus.Invalid:
        return BadRequest();
      case ResultStatus.NotFound:
        return NotFound();
      default:
        return StatusCode(500);
    }
  }
}

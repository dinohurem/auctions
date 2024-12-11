using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PomoziAuctions.Core.Aggregates.CompanyAggregate.Interfaces;
using PomoziAuctions.Core.Auth.Identity.Models;
using PomoziAuctions.Core.Auth.Identity.Services;
using PomoziAuctions.Core.Auth.Security.Interfaces;
using PomoziAuctions.Core.Auth.Security.Models;
using PomoziAuctions.SharedKernel.DataFilters;
using PomoziAuctions.Web.ApiModels.Auth;
using PomoziAuctions.Web.Extensions;

namespace PomoziAuctions.Web.Api;

public class AuthController : BaseApiController
{
  private readonly UserManager<Core.Auth.Identity.Models.IdentityUser> _userManager;
  private readonly RoleManager<Core.Auth.Identity.Models.IdentityRole> _roleManager;
  private readonly ITokenService _tokenService;
  private readonly IdentityService _identityService;
  private readonly IDataFilter _dataFilter;
  private readonly ICurrentUser _currentUser;

  public AuthController(UserManager<Core.Auth.Identity.Models.IdentityUser> userManager,
    RoleManager<Core.Auth.Identity.Models.IdentityRole> roleManager,
    ITokenService tokenService,
    IdentityService identityService,
    IDataFilter dataFilter,
    ICurrentUser currentUser)
  {
    _userManager = userManager;
    _roleManager = roleManager;
    _tokenService = tokenService;
    _identityService = identityService;
    _dataFilter = dataFilter;
    _currentUser = currentUser;
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] UserDto userDto)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest();
    }

    using var _ = _dataFilter.Disable<ICompanyKey>();
    var user = await _userManager.FindByEmailAsync(userDto.Email);
    if (user == null)
    {
      return Unauthorized();
    }

    var userRoles = await _userManager.GetRolesAsync(user);
    var userPermissions = await _roleManager.GetRolesPermissions(userRoles.ToArray());

    if (!CanLoginToCurrentApp(userPermissions) || user.IsDisabled)
    {
      return Unauthorized();
    }

    var validCredentials = await _userManager.CheckPasswordAsync(user, userDto.Password);
    if (!validCredentials)
    {
      return Unauthorized();
    }

    return Ok(new TokenDto
    {
      AccessToken = await _tokenService.GenerateAccessToken(user),
    });
  }

  [HttpPost("/refresh")]
  public async Task<IActionResult> Refresh(string email)
  {
    using var a = _dataFilter.Disable<ICompanyKey>();
    var user = await _userManager.FindByEmailAsync(email);
    if (user == null)
    {
      return Unauthorized();
    }

    return Ok(new TokenDto
    {
      AccessToken = await _tokenService.GenerateAccessToken(user)
    });
  }


  [HttpPost("/disable")]
  public async Task<IActionResult> Disable(string email)
  {
    using var a = _dataFilter.Disable<ICompanyKey>();
    var user = await _userManager.FindByEmailAsync(email);

    user.IsDisabled = true;
    await _userManager.UpdateAsync(user);

    return Ok();
  }

  [HttpPost("request-reset-password")]
  public async Task<IActionResult> RequestResetPassword([FromBody] RequestResetPasswordDto requestResetPasswordDto)
  {
    return MapResult(await _identityService.SendResetPasswordEmail(requestResetPasswordDto.Email, HttpContext.GetCulture()));
  }

  [HttpPost("reset-password")]
  public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
  {
    var resetPasswordResult = await _identityService.ResetPassword(resetPasswordDto);

    // Check if the result is false
    if (!resetPasswordResult)
    {
      // Return a BadRequest response with an error message
      return BadRequest(new { Error = "Password reset failed. Please check the provided information and try again." });
    }

    // If the result is true, map the result and return success response
    return Ok(new { Message = "Password has been reset successfully." });
  }

  private bool CanLoginToCurrentApp(IEnumerable<Permissions> userPermissions)
  {
    // Check if the user's permissions include either CanUseWebApp or AllAccess
    return userPermissions.Any(p => p == Permissions.CanUseWebApp || p == Permissions.AllAccess);
  }

  [HttpPost("change-password")]
  public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
  {

    var resetPasswordResult = await _identityService.ChangePassword(request);

    // Check if the result is false
    if (!resetPasswordResult)
    {
      // Return a BadRequest response with an error message
      return BadRequest(new { Error = "Password reset failed. Please check the provided information and try again." });
    }

    // If the result is true, map the result and return success response
    return Ok(new { Message = "Password has been reset successfully." });
  }

}

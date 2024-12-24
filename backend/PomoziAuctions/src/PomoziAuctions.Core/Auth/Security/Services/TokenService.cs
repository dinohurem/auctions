using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ardalis.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PomoziAuctions.Core.Auth.Security.Interfaces;
using PomoziAuctions.Core.Auth.Security.Models;

namespace PomoziAuctions.Core.Auth.Security.Services;

public class TokenService : ITokenService
{
  private readonly IOptions<Models.TokenOptions> _tokenOptions;
  private readonly UserManager<Identity.Models.IdentityUser> _userManager;
  private readonly RoleManager<Identity.Models.IdentityRole> _roleManager;

  public TokenService(IOptions<Models.TokenOptions> tokenOptions, UserManager<Identity.Models.IdentityUser> userManager, RoleManager<Identity.Models.IdentityRole> roleManager)
  {
    _tokenOptions = tokenOptions;
    _userManager = userManager;
    _roleManager = roleManager;
  }

  public async Task<Result<string>> GenerateAccessToken(Identity.Models.IdentityUser user, string companyId = null)
  {
    var claims = await GetUserClaims(user, companyId);

    return GetSecurityToken(claims);
  }

  private string GetSecurityToken(List<Claim> claims)
  {
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.Value.AccessTokenSecret));
    var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var now = DateTime.UtcNow;
    var securityToken = new JwtSecurityToken(
      _tokenOptions.Value.Issuer,
      _tokenOptions.Value.Audience,
      claims,
      now,
      now.Add(_tokenOptions.Value.AccessTokenDuration),
      signingCredentials);

    return new JwtSecurityTokenHandler().WriteToken(securityToken);
  }

  private async Task<List<Claim>> GetUserClaims(Identity.Models.IdentityUser user, string companyId = null)
  {
    var userRoles = await _userManager.GetRolesAsync(user);
    var claims = new List<Claim>()
    {
      new Claim(CustomClaimTypes.Email, user.Email),
      new Claim(CustomClaimTypes.UserId, user.Id),
      new Claim(CustomClaimTypes.AuctioneerId, user.AuctioneerId.ToString()),
      new Claim(CustomClaimTypes.Roles, string.Join(CustomClaimTypes.ClaimTypeValueSeparator, userRoles)),
    };

    var userPermissions = await _roleManager.GetRolesPermissions(userRoles.ToArray());
    var permissionsClaim = string.Join(CustomClaimTypes.ClaimTypeValueSeparator, userPermissions.Distinct().Select(p => (int)p));
    claims.Add(new Claim(CustomClaimTypes.Permissions, permissionsClaim));

    return claims;
  }
}

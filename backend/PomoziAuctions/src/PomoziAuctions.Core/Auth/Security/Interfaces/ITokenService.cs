using Ardalis.Result;
using PomoziAuctions.Core.Auth.Identity.Models;

namespace PomoziAuctions.Core.Auth.Security.Interfaces;

public interface ITokenService
{
  Task<Result<string>> GenerateAccessToken(IdentityUser user, string companyId = null);
}

using System.Security.Claims;

namespace PomoziAuctions.Core.Auth.Security.Interfaces;

public interface IClaimsPrincipalAccessor
{
  ClaimsPrincipal GetCurrentPrincipal();

  void SetCurrentPrincipal(ClaimsPrincipal claimsPrincipal);
}

using System.Security.Claims;
using PomoziAuctions.Core.Auth.Security.Interfaces;

namespace PomoziAuctions.Web.Security;

public class ClaimsPrincipalAccessor : IClaimsPrincipalAccessor
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public ClaimsPrincipalAccessor(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public ClaimsPrincipal GetCurrentPrincipal() => _httpContextAccessor.HttpContext.User;

	public void SetCurrentPrincipal(ClaimsPrincipal claimsPrincipal) => _httpContextAccessor.HttpContext.User = claimsPrincipal;
}

using Microsoft.AspNetCore.Authorization;
using PomoziAuctions.Core.Auth.Security.Interfaces;

namespace PomoziAuctions.Web.Security;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public PermissionHandler(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
	{
		var currentUser = _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<ICurrentUser>();

		if (currentUser.HasPermission(requirement.Permission))
		{
			context.Succeed(requirement);
		}

		return Task.CompletedTask;
	}
}

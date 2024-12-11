using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PomoziAuctions.Core.Auth.Security.Interfaces;
using PomoziAuctions.Core.Auth.Security.Models;

namespace PomoziAuctions.Web.Security;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddCurrentUserServices(this IServiceCollection services)
	{
		services.AddHttpContextAccessor();

		services.TryAddScoped<ICurrentUser, CurrentUser>();
		services.TryAddSingleton<IClaimsPrincipalAccessor, ClaimsPrincipalAccessor>();
		services.TryAddSingleton<IAuthorizationHandler, PermissionHandler>();
		services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

		return services;
	}
}

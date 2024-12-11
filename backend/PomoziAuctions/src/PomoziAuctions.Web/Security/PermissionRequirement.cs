using Microsoft.AspNetCore.Authorization;

namespace PomoziAuctions.Web.Security;

public class PermissionRequirement : IAuthorizationRequirement
{
	public PermissionRequirement(string permission)
	{
		Permission = permission;
	}

	public string Permission { get; }
}

using Microsoft.AspNetCore.Authorization;
using PomoziAuctions.Core.Auth.Security.Models;

namespace PomoziAuctions.Web.Security;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
public class PermissionGuardAttribute : AuthorizeAttribute
{
	public Permissions Permission { get; }

	public PermissionGuardAttribute(Permissions permission)
		: base(permission.ToString("D"))
	{
		Permission = permission;
	}
}

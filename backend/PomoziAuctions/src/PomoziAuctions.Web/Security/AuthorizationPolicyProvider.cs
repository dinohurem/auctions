﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace PomoziAuctions.Web.Security;

public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
	public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
	{
	}

	public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
	{
		return await base.GetPolicyAsync(policyName) ??
				new AuthorizationPolicyBuilder()
					   .AddRequirements(new PermissionRequirement(policyName))
					   .Build();
	}
}

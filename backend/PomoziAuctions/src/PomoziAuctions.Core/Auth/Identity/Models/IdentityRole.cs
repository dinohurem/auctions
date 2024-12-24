using Microsoft.AspNetCore.Identity;

namespace PomoziAuctions.Core.Auth.Identity.Models;

public class IdentityRole : IdentityRole<string>
{
}

public class IdentityRoleClaim : IdentityRoleClaim<string> { }

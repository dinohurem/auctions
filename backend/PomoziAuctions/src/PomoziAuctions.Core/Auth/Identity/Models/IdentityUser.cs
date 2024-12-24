using Microsoft.AspNetCore.Identity;

namespace PomoziAuctions.Core.Auth.Identity.Models;

public class IdentityUser : Microsoft.AspNetCore.Identity.IdentityUser
{
  public int? AuctioneerId { get; set; }

  public bool IsDisabled { get; set; } = false;
}

public class IdentityUserClaim : IdentityUserClaim<string> { }

public class IdentityUserLogin : IdentityUserLogin<string> { }

public class IdentityUserRole : IdentityUserRole<string> { }

public class IdentityUserToken : IdentityUserToken<string> { }

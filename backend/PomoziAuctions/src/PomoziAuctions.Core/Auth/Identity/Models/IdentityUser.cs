using Microsoft.AspNetCore.Identity;
using PomoziAuctions.Core.Aggregates.CompanyAggregate.Interfaces;

namespace PomoziAuctions.Core.Auth.Identity.Models;

public class IdentityUser : Microsoft.AspNetCore.Identity.IdentityUser, ICompanyKey
{
  public int? CompanyId { get; set; }

  public int? CandidateId { get; set; }

  public bool IsDisabled { get; set; } = false;
}

public class IdentityUserClaim : IdentityUserClaim<string> { }

public class IdentityUserLogin : IdentityUserLogin<string> { }

public class IdentityUserRole : IdentityUserRole<string> { }

public class IdentityUserToken : IdentityUserToken<string> { }

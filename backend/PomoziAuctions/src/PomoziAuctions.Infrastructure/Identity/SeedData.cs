using PomoziAuctions.Core.Auth.Identity.Models;
using PomoziAuctions.Core.Auth.Security.Models;
using IdentityRole = PomoziAuctions.Core.Auth.Identity.Models.IdentityRole;

namespace PomoziAuctions.Infrastructure.Identity;

public static class SeedData
{
  private const string SuperAdminRoleId = "5a16e63b-e9ad-4a2d-80c0-4f2ecdbf6219";
  private const string UserRoleId = "2b8f5331-6804-441e-8e9b-703eb8ef25cd";

  public static List<IdentityRole> GetRoles() => new()
  {
    new IdentityRole
    {
      Id = SuperAdminRoleId,
      Name = "Super Admin",
      NormalizedName = "SUPER ADMIN",
      ConcurrencyStamp = "cb270306-2cfa-49fa-86e0-9c2ff5782443"
    },   
    new IdentityRole
    {
      Id = UserRoleId,
      Name = "User",
      NormalizedName = "USER",
      ConcurrencyStamp = "9c21288f-13ed-4185-9310-a12b3c905abf"
    }    
  };

  public static List<IdentityRoleClaim> GetRoleClaims() => new()
  {
     // Super Admin role claims
     new IdentityRoleClaim
     {
         Id = 1,
         RoleId = SuperAdminRoleId,
         ClaimType = CustomClaimTypes.Permissions,
         ClaimValue = Permissions.AllAccess.ToString("D")
     },
     // User role claims
     new IdentityRoleClaim
     {
         Id = 2,
         RoleId = UserRoleId,
         ClaimType = CustomClaimTypes.Permissions,
         ClaimValue = Permissions.CanUseWebApp.ToString("D")
     },
  };

  public static List<IdentityUser> GetUsers() => new List<IdentityUser>
    {
        new IdentityUser
        {
            Id = "91BC21DA-87CA-4E56-BBE2-B8A9254A8B12", // Super Admin
            UserName = "superadmin",
            Email = "superadmin@sharklasers.com",
            NormalizedUserName = "SUPERADMIN",
            NormalizedEmail = "SUPERADMIN@SHARKLASERS.COM",
            IsDisabled = false,
            AuctioneerId = null // Super Admin has no CandidateId or CompanyId
        },
        new IdentityUser
        {
            Id = "4E4FBAAC-92CB-4BD6-8FCB-C0E678AAB4FE", // Ap Lab user
            UserName = "aplab",
            Email = "aplab@sharklasers.com",
            NormalizedUserName = "APLAB",
            NormalizedEmail = "APLAB@SHARKLASERS.COM",
            IsDisabled = false,
            AuctioneerId = 1 // Associate with Ap Lab auctioneer
        }
    };

  public static List<IdentityUserRole> GetUserRoles() => new List<IdentityUserRole>
  {
    new IdentityUserRole
    {
      UserId = "91BC21DA-87CA-4E56-BBE2-B8A9254A8B12", // Super Admin
      RoleId = SuperAdminRoleId
    },
    new IdentityUserRole
    {
      UserId = "4E4FBAAC-92CB-4BD6-8FCB-C0E678AAB4FE", // Ap Lab user
      RoleId = UserRoleId
    }   
  };

  public static List<IdentityUser> GetProdUsers() => new List<Core.Auth.Identity.Models.IdentityUser>
    {
        new IdentityUser
        {
            Id = "B70559AD-2B98-4C70-8540-71753B8680E2", // Super Admin
            UserName = "superadmin",
            Email = "superadmin@sharklasers.com",
            NormalizedUserName = "SUPERADMIN",
            NormalizedEmail = "SUPERADMIN@SHARKLASERS.COM",
            IsDisabled = false,
            AuctioneerId = null // Super Admin has no AuctioneerId
        },
    };

  public static List<IdentityUserRole> GetProdUserRoles() => new List<IdentityUserRole>
  {
    new IdentityUserRole
    {
      UserId = "B70559AD-2B98-4C70-8540-71753B8680E2", // Super Admin
      RoleId = SuperAdminRoleId
    }
  };
}

using PomoziAuctions.Core.Auth.Identity.Models;
using PomoziAuctions.Core.Auth.Security.Models;
using IdentityRole = PomoziAuctions.Core.Auth.Identity.Models.IdentityRole;

namespace PomoziAuctions.Infrastructure.Identity;

public static class SeedData
{
  private const string SuperAdminRoleId = "5a16e63b-e9ad-4a2d-80c0-4f2ecdbf6219";
  private const string CompanyAdminRoleId = "999f4c79-e3d4-4a3c-bf5a-650c1c880d39";
  private const string JobSeekerRoleId = "85af7d5e-6d6c-4f81-a239-9c2a81e5379b";
  private const string ParticipantRoleId = "2b8f5331-6804-441e-8e9b-703eb8ef25cd";
  private const string PanelistRoleId = "2b95ec50-81e6-4434-8fd9-c4174a282401";
  private const string VolunteerRoleId = "f56df030-c0ff-4ab4-b274-a10090c4b418";
  private const string ExhibitorRoleId = "fa14da30-9407-4152-b9e4-ef15b6e6613c";

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
      Id = CompanyAdminRoleId,
      Name = "Company Admin",
      NormalizedName = "COMPANY ADMIN",
      ConcurrencyStamp = "f22faa08-a603-4708-bbd4-32e6a7283c07"
    },
    new IdentityRole
    {
      Id = JobSeekerRoleId,
      Name = "Job Seeker",
      NormalizedName = "JOB SEEKER",
      ConcurrencyStamp = "d2efed57-c891-4f2b-9b9f-8cb8bafb4452"
    },
    new IdentityRole
    {
      Id = ParticipantRoleId,
      Name = "Participant",
      NormalizedName = "PARTICIPANT",
      ConcurrencyStamp = "9c21288f-13ed-4185-9310-a12b3c905abf"
    },
    new IdentityRole
    {
      Id = PanelistRoleId,
      Name = "Panelist",
      NormalizedName = "PANELIST",
      ConcurrencyStamp = "3d533b19-205c-4530-ab4b-1e8699aed702"
    },
    new IdentityRole
    {
      Id = VolunteerRoleId,
      Name = "Volunteer",
      NormalizedName = "VOLUNTEER",
      ConcurrencyStamp = "f9c6a5f7-21fe-43e1-b2a6-dcc63fd65fe2"
    },
    new IdentityRole
    {
      Id = ExhibitorRoleId,
      Name = "Exhibitor",
      NormalizedName = "EXHIBITOR",
      ConcurrencyStamp = "65626654-9e11-42d2-870a-16006f775854"
    }
  };

  public static List<IdentityRoleClaim> GetRoleClaims() => new()
  {
     // Super Admin role claims
     new IdentityRoleClaim
     {
         Id = 1, // Unique ID for the claim
         RoleId = "5a16e63b-e9ad-4a2d-80c0-4f2ecdbf6219", // Super Admin Role ID
         ClaimType = CustomClaimTypes.Permissions,
         ClaimValue = Permissions.AllAccess.ToString("D")
     },
     // Company Admin role claims
     new IdentityRoleClaim
     {
         Id = 2, // Unique ID for the claim
         RoleId = "999f4c79-e3d4-4a3c-bf5a-650c1c880d39", // Company Admin Role ID
         ClaimType = CustomClaimTypes.Permissions,
         ClaimValue = Permissions.CanUseWebApp.ToString("D")
     },
     // Job Seeker role claims
     new IdentityRoleClaim
     {
         Id = 3, // Unique ID for the claim
         RoleId = "85af7d5e-6d6c-4f81-a239-9c2a81e5379b", // Job Seeker Role ID
         ClaimType = CustomClaimTypes.Permissions,
         ClaimValue = Permissions.CanUseWebApp.ToString("D")
     },
     new IdentityRoleClaim
     {
         Id = 4, // Unique ID for the claim
         RoleId = "2b8f5331-6804-441e-8e9b-703eb8ef25cd", // Participant Role ID
         ClaimType = CustomClaimTypes.Permissions,
         ClaimValue = Permissions.CanUseWebApp.ToString("D")
     },
     new IdentityRoleClaim
     {
         Id = 5, // Unique ID for the claim
         RoleId = "2b95ec50-81e6-4434-8fd9-c4174a282401", // Panelist Role ID
         ClaimType = CustomClaimTypes.Permissions,
         ClaimValue = Permissions.CanUseWebApp.ToString("D")
     },
     new IdentityRoleClaim
     {
         Id = 6, // Unique ID for the claim
         RoleId = "f56df030-c0ff-4ab4-b274-a10090c4b418", // Volunteer Role ID
         ClaimType = CustomClaimTypes.Permissions,
         ClaimValue = Permissions.CanUseWebApp.ToString("D")
     }
  };

  public static List<IdentityUser> GetUsers() => new List<Core.Auth.Identity.Models.IdentityUser>
    {
        new IdentityUser
        {
            Id = "91BC21DA-87CA-4E56-BBE2-B8A9254A8B12", // Super Admin
            UserName = "superadmin",
            Email = "superadmin@example.com",
            NormalizedUserName = "SUPERADMIN",
            NormalizedEmail = "SUPERADMIN@EXAMPLE.COM",
            IsDisabled = false,
            CompanyId = null,
            CandidateId = null // Super Admin has no CandidateId or CompanyId
        },
        new Core.Auth.Identity.Models.IdentityUser
        {
            Id = "4E4FBAAC-92CB-4BD6-8FCB-C0E678AAB4FE", // Candidate user for John Doe
            UserName = "john_doe_user",
            Email = "john.doe@example.com",
            NormalizedUserName = "JOHN_DOE_USER",
            NormalizedEmail = "JOHN.DOE@EXAMPLE.COM",
            IsDisabled = false,
            CompanyId = null, // Candidate does not have a CompanyId
            CandidateId = 1 // Associate with John Doe Candidate
        },
        new PomoziAuctions.Core.Auth.Identity.Models.IdentityUser
        {
            Id = "632EDDBF-4A79-4E13-8BF8-A721B9163EF2", // Candidate user for Jane Smith
            UserName = "jane_smith_user",
            Email = "jane.smith@example.com",
            NormalizedUserName = "JANE_SMITH_USER",
            NormalizedEmail = "JANE.SMITH@EXAMPLE.COM",
            IsDisabled = false,
            CompanyId = null, // Candidate does not have a CompanyId
            CandidateId = 2 // Associate with Jane Smith Candidate
        },
        new Core.Auth.Identity.Models.IdentityUser
        {
            Id = "9D07475B-9CB2-49CD-8F62-2ECCA97F6FFF", // Company user for Amazon
            UserName = "amazon_admin",
            Email = "admin@amazon.com",
            NormalizedUserName = "AMAZON_ADMIN",
            NormalizedEmail = "ADMIN@AMAZON.COM",
            IsDisabled = false,
            CompanyId = 1, // Associate with Amazon
            CandidateId = null // Not a candidate
        },
        new Core.Auth.Identity.Models.IdentityUser
        {
            Id = "B71D94C0-DA70-4B67-BF04-A517A46EF69D", // Company user for Google
            UserName = "google_admin",
            Email = "admin@google.com",
            NormalizedUserName = "GOOGLE_ADMIN",
            NormalizedEmail = "ADMIN@GOOGLE.COM",
            IsDisabled = false,
            CompanyId = 2, // Associate with Google
            CandidateId = null // Not a candidate
        },
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
      UserId = "4E4FBAAC-92CB-4BD6-8FCB-C0E678AAB4FE", // Candidate user for John Doe
      RoleId = JobSeekerRoleId
    },
    new IdentityUserRole
    {
      UserId = "632EDDBF-4A79-4E13-8BF8-A721B9163EF2", // Candidate user for Jane Smith
      RoleId = JobSeekerRoleId
    },
    new IdentityUserRole
    {
      UserId = "9D07475B-9CB2-49CD-8F62-2ECCA97F6FFF", // Company user for Amazon
      RoleId = CompanyAdminRoleId
    },
    new IdentityUserRole
    {
      UserId = "B71D94C0-DA70-4B67-BF04-A517A46EF69D", // Company user for Google
      RoleId = CompanyAdminRoleId
    }
  };

  public static List<IdentityUser> GetProdUsers() => new List<Core.Auth.Identity.Models.IdentityUser>
    {
        new IdentityUser
        {
            Id = "B70559AD-2B98-4C70-8540-71753B8680E2", // Super Admin
            UserName = "superadmin",
            Email = "superadmin@example.com",
            NormalizedUserName = "SUPERADMIN",
            NormalizedEmail = "SUPERADMIN@EXAMPLE.COM",
            IsDisabled = false,
            CompanyId = null,
            CandidateId = null // Super Admin has no CandidateId or CompanyId
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

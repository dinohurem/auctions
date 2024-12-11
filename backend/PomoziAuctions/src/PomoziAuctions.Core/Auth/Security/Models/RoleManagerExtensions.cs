using Microsoft.AspNetCore.Identity;
using PomoziAuctions.Core.Extensions;

namespace PomoziAuctions.Core.Auth.Security.Models;
public static class RoleManagerExtensions
{
  public static async Task<IEnumerable<Permissions>> GetRolesPermissions(this RoleManager<Identity.Models.IdentityRole> roleManager, params string[] roleNames)
  {
    var userPermissions = new List<Permissions>();
    foreach (var roleName in roleNames)
    {
      var role = await roleManager.FindByNameAsync(roleName);
      var rolePermissions = (await roleManager.GetClaimsAsync(role)).Where(c => c.Type == CustomClaimTypes.Permissions);

      userPermissions.AddRange(rolePermissions.Select(p => p.Value.ToEnum<Permissions>()));
    }

    return userPermissions;
  }
}

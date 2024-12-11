using System.ComponentModel.DataAnnotations;

namespace PomoziAuctions.Core.Auth.Security.Models;

public enum Permissions : ushort
{
  [Display(GroupName = "User", Name = "Use web app", Description = "Can login and use web app.")]
  CanUseWebApp = 0x30,

  [Display(GroupName = "SuperAdmin", Name = "AllAccess", Description = "Allows user to access to all features.", AutoGenerateField = false)]
  AllAccess = ushort.MaxValue
}

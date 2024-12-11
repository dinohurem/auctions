using System.ComponentModel.DataAnnotations;

namespace PomoziAuctions.Core.Auth.Identity.Models;

public class ChangePasswordDto
{
  [Required]
  public string CurrentPassword { get; set; }

  [Required]
  public string NewPassword { get; set; }

  [Required]
  public string ConfirmNewPassword { get; set; }
}

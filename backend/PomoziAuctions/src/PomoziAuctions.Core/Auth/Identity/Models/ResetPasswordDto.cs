using System.ComponentModel.DataAnnotations;

namespace PomoziAuctions.Core.Auth.Identity.Models;

public class ResetPasswordDto
{
  [Required]
  public string Email { get; set; }

  [Required]
  public string Token { get; set; }

  [Required]
  public string NewPassword { get; set; }

  public bool IsNewUser { get; set; }
}

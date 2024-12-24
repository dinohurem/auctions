using System.ComponentModel.DataAnnotations;

namespace PomoziAuctions.Core.Auth.Identity.Models;

public class RequestResetPasswordDto
{
  [Required]
  public string Email { get; set; }
}

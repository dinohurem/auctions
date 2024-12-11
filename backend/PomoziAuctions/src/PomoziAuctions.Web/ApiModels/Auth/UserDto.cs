using System.ComponentModel.DataAnnotations;

namespace PomoziAuctions.Web.ApiModels.Auth;

public class UserDto
{
  [Required]
  public string Email { get; set; }

  [Required]
  public string Password { get; set; }
}

namespace PomoziAuctions.Core.Auth.Identity.Models;

public class AcceptInvitationDto
{
  public string Code { get; set; }

  public string Email { get; set; }

  public string Password { get; set; }

  public string PasswordConfirmation { get; set; }
}

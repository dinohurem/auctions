namespace PomoziAuctions.Core.Auth.Identity.Models;

public class IdentityEmailOptions
{
  public string FromEmail { get; set; }

  public string ResetPasswordUrl { get; set; }

  public string WebUrl { get; set; }

  public string WelcomeEmailTemplate { get; set; }

  public string PasswordRecoveryTemplate { get; set; }

  public string AuctioneerInfoTemplate { get; set; }
}

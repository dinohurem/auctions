namespace PomoziAuctions.Core.Auth.Identity.Models;

public class IdentityEmailOptions
{
  public string FromEmail { get; set; }

  public string ResetPasswordUrl { get; set; }

  public string WebUrl { get; set; }

  public string GooglePlaystoreAppUrl { get; set; }

  public string AppleStoreAppUrl { get; set; }

  public string BrowserExtensionUrl { get; set; }

  public string WelcomeEmailTemplate { get; set; }

  public string PasswordRecoveryTemplate { get; set; }

  public string AcceptInviteTemplate { get; set; }

  public string JobInfoTemplate { get; set; }

  public string CandidateInfoTemplate { get; set; }
}

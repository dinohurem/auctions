namespace PomoziAuctions.Core.Auth.Security.Models;

public class TokenOptions
{
  public string Issuer { get; set; }

  public string Audience { get; set; }

  public TimeSpan AccessTokenDuration { get; set; }

  public string AccessTokenSecret { get; set; }
}

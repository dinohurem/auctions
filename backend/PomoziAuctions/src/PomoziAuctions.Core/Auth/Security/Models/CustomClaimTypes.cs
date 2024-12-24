namespace PomoziAuctions.Core.Auth.Security.Models;

public static class CustomClaimTypes
{
  /// <summary>
  ///     Gets or sets claim type value separator.
  /// </summary>
  /// <remarks>
  ///     Used for claim type values that can be comma spearated strings or space separated strings.
  /// </remarks>
  /// <example>
  ///     Used for permissions and roles.
  /// </example>
  public static string ClaimTypeValueSeparator { get; set; } = ",";

  public static string Email { get; set; } = "email";

  public static string UserId { get; set; } = "useridentifier";

  public static string AuctioneerId { get; set; } = "auctioneeridentifier";

  public static string Permissions { get; set; } = "permissions";

  public static string Roles { get; set; } = "roles";

  public static string UserImpersonation { get; set; } = "userimpersonation";
}

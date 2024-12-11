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

  public static string CandidateId { get; set; } = "candidateidentifier";

  public static string Permissions { get; set; } = "permissions";

  public static string Roles { get; set; } = "roles";

  public static string CompanyId { get; set; } = "companyidentifier";

  public static string CompanyName { get; set; } = "companyname";

  public static string CompanyType { get; set; } = "companytype";

  public static string UserImpersonation { get; set; } = "userimpersonation";
}

using System.Security.Claims;
using PomoziAuctions.Core.Auth.Security.Interfaces;

namespace PomoziAuctions.Core.Auth.Security.Models;

public class CurrentUser : ICurrentUser
{
  private readonly IClaimsPrincipalAccessor _claimsPrincipalAccessor;
  private readonly Lazy<ClaimsPrincipal> _principal;

  private Guid? _id;
  private int? _companyId;
  private Guid? _packageId;
  private string _email;
  private IReadOnlyCollection<string> _roles;
  private IReadOnlyCollection<string> _permissions;
  private IReadOnlyCollection<Claim> _userClaims;

  public CurrentUser(IClaimsPrincipalAccessor claimsPrincipalAccessor)
  {
    _claimsPrincipalAccessor = claimsPrincipalAccessor;
    _principal = new Lazy<ClaimsPrincipal>(() => _claimsPrincipalAccessor.GetCurrentPrincipal());
  }

  public bool IsAuthenticated => Id != null;

  public Guid? Id => _id ??= ToGuid(GetClaimValue(CustomClaimTypes.UserId));

  public int? CompanyId => _companyId ??= ToInt(GetClaimValue(CustomClaimTypes.CompanyId));

  public string Email => _email ??= GetClaimValue(CustomClaimTypes.Email);

  /// <inheritdoc/>
  public IReadOnlyCollection<string> Roles =>
    _roles ??= GetClaimValue(CustomClaimTypes.Roles)?
            .Split(CustomClaimTypes.ClaimTypeValueSeparator.ToArray())
            .Select(p => p.Trim()).ToList().AsReadOnly();

  /// <inheritdoc/>
  public IReadOnlyCollection<string> Permissions =>
    _permissions ??= GetClaimValue(CustomClaimTypes.Permissions)?
              .Split(CustomClaimTypes.ClaimTypeValueSeparator.ToArray())
              .Select(p => p.Trim()).ToList().AsReadOnly();

  protected ClaimsPrincipal Principal => _principal.Value;

  public Claim GetUserClaimOrDefault(string claimName) =>
    GetUserClaims().FirstOrDefault(c => c.Type == claimName);

  public IReadOnlyCollection<Claim> GetUserClaims() =>
    _userClaims ??= Principal.Claims?.ToList().AsReadOnly() ?? Enumerable.Empty<Claim>().ToList().AsReadOnly();

  public IReadOnlyCollection<Claim> GetUserClaims(string claimName) =>
    GetUserClaims().Where(c => c.Type == claimName).ToList().AsReadOnly() ?? Enumerable.Empty<Claim>().ToList().AsReadOnly();

  public bool HasPermission(string permission) =>
    Permissions?.Any(p => p != null && permission != null && p.ToUpperInvariant() == permission.ToUpperInvariant()) ?? false;

  public bool HasRole(string role) =>
    Roles.Any(r => r != null && role != null && r.ToUpperInvariant() == role.ToUpperInvariant());

  private static Guid? ToGuid(string value) =>
    string.IsNullOrWhiteSpace(value) ? null : Guid.TryParse(value, out var guid) ? guid : null;

  private static int? ToInt(string value) =>
    string.IsNullOrWhiteSpace(value) ? null : int.TryParse(value, out var guid) ? guid : null;

  private string GetClaimValue(string claimName) => GetUserClaimOrDefault(claimName)?.Value;
}

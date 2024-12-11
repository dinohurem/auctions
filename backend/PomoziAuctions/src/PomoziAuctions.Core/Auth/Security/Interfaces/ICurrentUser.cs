using System.Security.Claims;

namespace PomoziAuctions.Core.Auth.Security.Interfaces;

public interface ICurrentUser
{
  bool IsAuthenticated { get; }

  Guid? Id { get; }

  int? CompanyId { get; }

  string Email { get; }

  IReadOnlyCollection<string> Roles { get; }

  IReadOnlyCollection<Claim> GetUserClaims();

  IReadOnlyCollection<Claim> GetUserClaims(string claimName);

  Claim GetUserClaimOrDefault(string claimName);

  bool HasPermission(string permission);

  bool HasRole(string role);
}

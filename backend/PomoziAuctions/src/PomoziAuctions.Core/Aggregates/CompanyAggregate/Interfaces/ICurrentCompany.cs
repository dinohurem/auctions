namespace PomoziAuctions.Core.Aggregates.CompanyAggregate.Interfaces;

public interface ICurrentCompany
{
  int? Id { get; }

  string Name { get; }

  Guid? PackageId { get; }

  IDisposable SetCurrentCompany(int? id, string name, string type, Guid? packageId = null);
}

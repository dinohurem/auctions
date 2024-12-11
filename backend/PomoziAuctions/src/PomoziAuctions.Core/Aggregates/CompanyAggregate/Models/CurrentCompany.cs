using PomoziAuctions.Core.Aggregates.CompanyAggregate.Interfaces;
using PomoziAuctions.SharedKernel;

namespace PomoziAuctions.Core.Aggregates.CompanyAggregate.Models;

public class CurrentCompany : ICurrentCompany
{
  private readonly ICurrentCompanyAccessor _currentCompanyAccessor;
  private Lazy<CompanyInfo> _companyInfo;

  public CurrentCompany(ICurrentCompanyAccessor currentCompanyAccessor)
  {
    _currentCompanyAccessor = currentCompanyAccessor;
    _companyInfo = new Lazy<CompanyInfo>(() => _currentCompanyAccessor.GetCurrentCompany() ?? new CompanyInfo());
  }

  protected CompanyInfo CompanyInfo => _companyInfo.Value;

  public int? Id => CompanyInfo.Id;

  public string Name => CompanyInfo.Name;

  public string Type => CompanyInfo.Type;

  public Guid? PackageId => CompanyInfo.PackageId;

  public IDisposable SetCurrentCompany(int? id, string name, string type, Guid? packageId = null)
  {
    _companyInfo = new Lazy<CompanyInfo>(() => new CompanyInfo
    {
      Id = id,
      Name = name,
      Type = type,
    });

    return new DisposableAction(() =>
    {
      _companyInfo = new Lazy<CompanyInfo>(() => _currentCompanyAccessor.GetCurrentCompany() ?? new CompanyInfo());
    });
  }
}

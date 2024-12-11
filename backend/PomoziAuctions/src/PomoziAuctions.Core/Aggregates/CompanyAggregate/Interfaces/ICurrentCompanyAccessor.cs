using PomoziAuctions.Core.Aggregates.CompanyAggregate.Models;

namespace PomoziAuctions.Core.Aggregates.CompanyAggregate.Interfaces;

public interface ICurrentCompanyAccessor
{
  CompanyInfo GetCurrentCompany();
}

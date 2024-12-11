using Ardalis.Result;
using PomoziAuctions.Core.Aggregates.CompanyAggregate.Models;

namespace PomoziAuctions.Core.Aggregates.CompanyAggregate.Interfaces;

public interface ICompanyRegistrationService
{
  Task<Result<CompanyDto>> Register(RegisterCompanyDto registrationDto);

  Task<Result<bool>> Approve(int companyId);
}

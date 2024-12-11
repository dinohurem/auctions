using Ardalis.Result;
using PomoziAuctions.Core.Aggregates.CompanyAggregate.Models;
using PomoziAuctions.SharedKernel.Specifications;

namespace PomoziAuctions.Core.Aggregates.CompanyAggregate.Interfaces;

public interface ICompanyService
{
  Task<Result<CompanyDto>> CreateCompany(CompanyDto company);

  Task<Result<CompanyDto>> UpdateCompany(CompanyDto company);

  Task<Result<CompanyDto>> Delete(int id);

  Task<Result<List<CompanyDto>>> GetAllCompanies();

  Task<Result<CompanyDto>> Get(int companyId);

  Task<Result<PaginatedResult<CompanyDto>>> GetCompaniesFilteredPaginatedAsync(FilterCompanyDto filter);
}

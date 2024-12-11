using Ardalis.Result;
using AutoMapper;
using CompanyFair.Core.Aggregates.CompanyAggregate.Specifications;
using PomoziAuctions.Core.Aggregates.BlobAggregate.Interfaces;
using PomoziAuctions.Core.Aggregates.CompanyAggregate.Interfaces;
using PomoziAuctions.Core.Aggregates.CompanyAggregate.Models;
using PomoziAuctions.Core.Aggregates.CompanyAggregate.Specifications;
using PomoziAuctions.Core.Auth.Identity.Services;
using PomoziAuctions.SharedKernel.Interfaces;
using PomoziAuctions.SharedKernel.Specifications;

namespace PomoziAuctions.Core.Aggregates.CompanyAggregate.Services;

public class CompanyService : ICompanyService
{
  private readonly IdentityService _identityService;
  private readonly IRepository<Company> _repository;
  private readonly IMapper _mapper;
  private readonly IBlobService _blobService;
  private readonly IBlobStorage _blobStorage;

  public CompanyService(IdentityService identityService, IRepository<Company> repository, IMapper mapper, IBlobService blobService, IBlobStorage blobStorage)
  {
    _identityService = identityService;
    _repository = repository;
    _mapper = mapper;
    _blobService = blobService;
    _blobStorage = blobStorage;
  }

  public async Task<Result<CompanyDto>> CreateCompany(CompanyDto companyDto)
  {
    var newCompany = _mapper.Map<Company>(companyDto);
    var company = await _repository.AddAsync(newCompany);

    return new Result<CompanyDto>(_mapper.Map<CompanyDto>(company));
  }

  public async Task<Result<CompanyDto>> UpdateCompany(CompanyDto companyDto)
  {
    var companyByIdSpec = new SearchCompanyByIdSpec(companyDto.Id);

    var company = await _repository.FirstOrDefaultAsync(companyByIdSpec);

    if (company == null)
    {
      return Result<CompanyDto>.NotFound();
    }

    _mapper.Map(companyDto, company);
    await _repository.UpdateAsync(company);

    // Check if the email has changed and update it in the identity service.
    if (companyDto.ContactEmail != company.ContactEmail)
    {
      var updatedEmailResult = await _identityService.UpdateUserEmail(company.ContactEmail, companyDto.ContactEmail);

      if (!updatedEmailResult.Value)
      {
        // Email update was unsuccessful
        return Result<CompanyDto>.Error("Error encountered while updating email.");
      }
    }


    // Delete blob.
    if (companyDto.ImageUrl != null)
    {
      var blobToDelete = await _blobService.DeleteAsync(company.BlobId.Value);
      if (blobToDelete.Value != null)
      {
        await _blobStorage.DeleteAsync(blobToDelete.Value.NormalizedName);
      }
    }

    // Upload blob.
    if (companyDto.File != null)
    {
      var blob = await _blobStorage.UploadAsync(companyDto.File);
      if (blob != null)
      {
        var blobDto = blob.Blob;
        var createdBlob = await _blobService.CreateAsync(blobDto);
        if (createdBlob != null)
        {
          company.BlobId = createdBlob.Value.Id;
          await _repository.UpdateAsync(company);
        }
      }
    }

    var dto = _mapper.Map<CompanyDto>(company);

    if (company.Blob != null)
    {
      // Construct the full blob URL using BlobClient
      var blobUrl = _blobStorage.GetBlobUrlWithSas(company.Blob.NormalizedName, TimeSpan.FromHours(5));
      dto.ImageUrl = blobUrl;
    }

    return new Result<CompanyDto>(dto);
  }

  public async Task<Result<CompanyDto>> Delete(int id)
  {
    var spec = new SearchCompanyByIdSpec(id);
    var company = await _repository.FirstOrDefaultAsync(spec);

    if (company == null)
    {
      return Result<CompanyDto>.NotFound();
    }

    if (company.BlobId != null)
    {
      var blobToDelete = await _blobService.DeleteAsync(company.BlobId.Value);
      if (blobToDelete.Value != null)
      {
        await _blobStorage.DeleteAsync(blobToDelete.Value.NormalizedName);
      }
    }

    company.Deleted = true;
    await _repository.UpdateAsync(company);

    return new Result<CompanyDto>(_mapper.Map<CompanyDto>(company));
  }

  public async Task<Result<List<CompanyDto>>> GetAllCompanies()
  {
    var companies = await _repository.ListAsync();

    return new Result<List<CompanyDto>>(_mapper.Map<List<CompanyDto>>(companies));
  }

  public async Task<Result<CompanyDto>> Get(int companyId)
  {
    var companyByIdSpec = new SearchCompanyByIdSpec(companyId);
    var company = await _repository.FirstOrDefaultAsync(companyByIdSpec);

    if (company == null)
    {
      return Result<CompanyDto>.NotFound();
    }

    var dto = _mapper.Map<CompanyDto>(company);
    if (company.Blob != null)
    {
      // Construct the full blob URL using BlobClient
      var blobUrl = _blobStorage.GetBlobUrlWithSas(company.Blob.NormalizedName, TimeSpan.FromHours(5));
      dto.ImageUrl = blobUrl;
    }

    return dto;
  }

  public async Task<Result<PaginatedResult<CompanyDto>>> GetCompaniesFilteredPaginatedAsync(FilterCompanyDto filter)
  {
    var spec = new AllCompaniesFilteredPaginatedSpec(filter);
    var companies = await _repository.ListAsync(spec);
    var totalCount = await _repository.CountAsync(new AllCompaniesSpec());

    var companyDtos = companies.Select(company =>
    {
      var dto = _mapper.Map<CompanyDto>(company);
      if (company.Blob != null)
      {
        // Construct the full blob URL using BlobClient
        var blobUrl = _blobStorage.GetBlobUrlWithSas(company.Blob.NormalizedName, TimeSpan.FromHours(5));
        dto.ImageUrl = blobUrl;
      }
      return dto;
    }).ToList();

    return new PaginatedResult<CompanyDto>(companyDtos, totalCount, filter.PageNumber, filter.PageSize);
  }
}

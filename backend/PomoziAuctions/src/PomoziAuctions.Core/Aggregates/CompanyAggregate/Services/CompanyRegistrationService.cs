using Ardalis.Result;
using AutoMapper;
using PomoziAuctions.Core.Aggregates.BlobAggregate.Interfaces;
using PomoziAuctions.Core.Aggregates.CompanyAggregate.Interfaces;
using PomoziAuctions.Core.Aggregates.CompanyAggregate.Models;
using PomoziAuctions.Core.Auth.Identity.Models;
using PomoziAuctions.Core.Auth.Identity.Services;
using PomoziAuctions.SharedKernel.Interfaces;

namespace PomoziAuctions.Core.Aggregates.CompanyAggregate.Services;

public class CompanyRegistrationService : ICompanyRegistrationService
{
  private readonly ICurrentCompany _currentCompany;
  private readonly IRepository<Company> _repository;
  private readonly IdentityService _identityService;
  private readonly IBlobService _blobService;
  private readonly IBlobStorage _blobStorage;
  private readonly IMapper _mapper;
  private static readonly string _companyAdminRole = "Company Admin";


  public CompanyRegistrationService(ICurrentCompany currentCompany, IRepository<Company> repository, IdentityService identityService, IMapper mapper, IBlobService blobService, IBlobStorage blobStorage)
  {
    _currentCompany = currentCompany;
    _repository = repository;
    _identityService = identityService;
    _blobService = blobService;
    _blobStorage = blobStorage;
    _mapper = mapper;
  }

  public async Task<Result<bool>> Approve(int companyId)
  {
    var company = await _repository.GetByIdAsync(companyId);

    if (company == null || company.PendingApproval.GetValueOrDefault(false) == false)
    {
      return new Result<bool>(false);
    }

    company.PendingApproval = false;
    await _repository.UpdateAsync(company);

    return await _identityService.SendInvitationToExistingUser(companyId, company.ContactEmail);
  }

  public async Task<Result<CompanyDto>> Register(RegisterCompanyDto registrationDto)
  {

    var newCompany = _mapper.Map<Company>(registrationDto);

    var company = await _repository.AddAsync(newCompany);

    // Upload blob.
    if (registrationDto.File != null)
    {
      var blob = await _blobStorage.UploadAsync(registrationDto.File);
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

    await _identityService.SendInvitation(new InviteUserDto
    {
      Email = company.ContactEmail,
      CompanyId = company.Id,
      Roles = [_companyAdminRole],
      Name = company.Name,
    });

    return new Result<CompanyDto>(_mapper.Map<CompanyDto>(company));
  }
}

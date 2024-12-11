using AutoMapper;
using PomoziAuctions.Core.Aggregates.BlobAggregate.Models;
using PomoziAuctions.Core.Aggregates.CompanyAggregate.Models;
using PomoziAuctions.Core.Aggregates.TabAggregate.Models;

namespace PomoziAuctions.Web.Configuration;
/// <summary>
/// Represents the AutoMapper profile for mapping entities to DTOs and vice versa.
/// </summary>
public class AutoMappingProfile : Profile
{
  /// <summary>
  /// Represents the AutoMapper profile for mapping entities to DTOs and vice versa.
  /// </summary>
  public AutoMappingProfile()
  {
    MapGeneral();
  }

  private void MapGeneral()
  {
    CreateMap<Tab, TabDto>().ReverseMap();
    CreateMap<Company, CompanyDto>().ReverseMap();
    CreateMap<BulkImportCompanyDto, Company>();

    CreateMap<CreateBlobDto, Blob>()
      .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FileName))
      .ForMember(dest => dest.NormalizedName, opt => opt.MapFrom(src => src.NormalizedName));

    CreateMap<Blob, BlobDto>().ReverseMap();   

    // Company Mapping
    CreateMap<Company, CompanyDto>()
      .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Blob != null ? src.Blob.NormalizedName : null))
      .ReverseMap();
    CreateMap<RegisterCompanyDto, Company>()
      .ForMember(dest => dest.ContactNumber, opt => opt.MapFrom(src => src.Phone))
      .ForMember(dest => dest.ContactPersonFirstName, opt => opt.MapFrom(src => src.FirstName))
      .ForMember(dest => dest.ContactPersonLastName, opt => opt.MapFrom(src => src.LastName))
      .ForMember(dest => dest.RegistrationNumber, opt => opt.MapFrom(src => src.RegistrationNumber))
      .ForMember(dest => dest.ContactEmail, opt => opt.MapFrom(src => src.Email))
      .ForMember(dest => dest.CompanyType, opt => opt.MapFrom(src => src.CompanyType));


    // Blob Mapping
    CreateMap<Blob, BlobDto>().ReverseMap();    
  }

  private TimeOnly ParseTime(string timeString)
  {
    // Parse string (e.g., "HH:mm") to TimeOnly
    if (TimeOnly.TryParse(timeString, out var timeOnly))
    {
      return timeOnly;
    }
    throw new ArgumentException($"Invalid time format: {timeString}");
  }
}



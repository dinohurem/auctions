using AutoMapper;
using PomoziAuctions.Core.Aggregates.BlobAggregate.Models;

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
    CreateMap<CreateBlobDto, Blob>()
      .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FileName))
      .ForMember(dest => dest.NormalizedName, opt => opt.MapFrom(src => src.NormalizedName));

    CreateMap<Blob, BlobDto>().ReverseMap(); 

    // Blob Mapping
    CreateMap<Blob, BlobDto>().ReverseMap();    
  }
}



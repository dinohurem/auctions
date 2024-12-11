using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace PomoziAuctions.Core.Aggregates.CompanyAggregate.Models;
public class CompanyDto
{
  public int Id { get; set; }

  public string Name { get; set; }

  public string Industry { get; set; }

  public string Country { get; set; }

  public string Address { get; set; }

  public string City { get; set; }

  public string County { get; set; }

  [CanBeNull]
  public string State { get; set; }

  public string Zip { get; set; }

  public string ContactPersonFirstName { get; set; }

  public string ContactPersonLastName { get; set; }

  public string ContactNumber { get; set; }

  public string ContactEmail { get; set; }

  public string Website { get; set; }

  public SharedKernel.Enums.CompanyType CompanyType { get; set; }

  public int? ParentId { get; set; }

  public string ImageUrl { get; set; }

  [CanBeNull] public IFormFile File { get; set; }

  public string RegistrationNumber { get; set; }
}

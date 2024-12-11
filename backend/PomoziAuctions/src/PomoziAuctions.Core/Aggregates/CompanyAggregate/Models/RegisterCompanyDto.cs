using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace PomoziAuctions.Core.Aggregates.CompanyAggregate.Models;

public class RegisterCompanyDto
{
  [Required]
  public string Name { get; set; }

  [Required]
  [EmailAddress]
  public string Email { get; set; }

  public string City { get; set; }

  public string Industry { get; set; }

  public string Phone { get; set; }

  public string FirstName { get; set; }

  public string LastName { get; set; }

  public string CompanyType { get; set; }

  public string RegistrationNumber { get; set; }

  public ICollection<string> Roles { get; set; }

  [CanBeNull] public IFormFile File { get; set; }
}

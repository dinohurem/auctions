using PomoziAuctions.Core.Aggregates.BlobAggregate.Models;
using PomoziAuctions.SharedKernel;
using PomoziAuctions.SharedKernel.Interfaces;

namespace PomoziAuctions.Core.Aggregates.CompanyAggregate.Models;

public class Company : EntityBase, IAggregateRoot, ISoftDelete
{
  public string Name { get; set; }

  public string City { get; set; }

  public string ContactPersonFirstName { get; set; }

  public string ContactPersonLastName { get; set; }

  public string ContactNumber { get; set; }

  public string ContactEmail { get; set; }

  public string Industry { get; set; }

  public string Website { get; set; }

  public int? ParentId { get; set; }

  public bool? PendingApproval { get; set; } = false;

  public SharedKernel.Enums.CompanyType CompanyType { get; set; }
  public bool Deleted { get; set; }

  public int? BlobId { get; set; }
  public Blob Blob { get; set; }

  public string RegistrationNumber { get; set; }
}

using JetBrains.Annotations;

namespace PomoziAuctions.Core.Aggregates.CompanyAggregate.Models;
public class BulkImportCompanyDto
{

  public string Name { get; set; }

  public string Industry { get; set; }


  public string City { get; set; }


  public string ContactEmail { get; set; }


  public SharedKernel.Enums.CompanyType CompanyType { get; set; } = SharedKernel.Enums.CompanyType.Regular;

}

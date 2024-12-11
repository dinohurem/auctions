using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PomoziAuctions.Core.Aggregates.CompanyAggregate.Models;
using PomoziAuctions.SharedKernel.Extensions;

namespace PomoziAuctions.Infrastructure.Data.Config;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
  public void Configure(EntityTypeBuilder<Company> builder)
  {
    builder.Property(c => c.Name)
      .HasMaxLength(100)
      .IsRequired();

    builder.Property(c => c.ContactPersonFirstName)
      .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);

    builder.Property(c => c.ContactPersonLastName)
      .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);

    builder.Property(c => c.ContactNumber)
      .HasMaxLength(20);

    builder.Property(c => c.ContactEmail)
      .HasMaxLength(50)
      .IsRequired();

    builder.Property(c => c.Website)
      .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);

    builder.Property(p => p.CompanyType).HasConversion(v => v.ToString(), v => v.ToEnum<SharedKernel.Enums.CompanyType>(false));
  }
}

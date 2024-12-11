using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PomoziAuctions.Core.Aggregates.BlobAggregate.Models;

namespace PomoziAuctions.Infrastructure.Data.Config;

public class BlobConfiguration : IEntityTypeConfiguration<Blob>
{
	public void Configure(EntityTypeBuilder<Blob> builder)
	{
		builder.Property(x => x.Name)
			.HasMaxLength(1024)
			.IsRequired();

		builder.Property(x => x.NormalizedName)
			.HasMaxLength(1024)
			.IsRequired();

		builder.Property(c => c.Deleted)
			.HasDefaultValue(false);
	}
}

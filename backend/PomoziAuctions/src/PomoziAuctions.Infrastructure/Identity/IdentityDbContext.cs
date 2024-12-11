using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PomoziAuctions.Core.Aggregates.CompanyAggregate.Interfaces;
using PomoziAuctions.Core.Auth.Identity.Models;
using PomoziAuctions.SharedKernel;
using PomoziAuctions.SharedKernel.DataFilters;

namespace PomoziAuctions.Infrastructure.Identity;

public class IdentityDbContext : IdentityDbContext<IdentityUser, IdentityRole, string, IdentityUserClaim, IdentityUserRole, IdentityUserLogin, IdentityRoleClaim, IdentityUserToken>
{
  private readonly IDataFilter _dataFilter;
  private readonly ICurrentCompany _currentCompany;

  public IdentityDbContext(DbContextOptions dbContextOptions)
    : base(dbContextOptions)
  {
  }

  public IdentityDbContext(DbContextOptions dbContextOptions, IDataFilter dataFilter, ICurrentCompany currentCompany)
    : base(dbContextOptions)
  {
    _dataFilter = dataFilter;
    _currentCompany = currentCompany;
  }

  protected bool SoftDeleteFilterEnabled => _dataFilter?.IsEnabled<ISoftDelete>() ?? true;

  protected bool CompanyKeyFilterEnabled => _dataFilter?.IsEnabled<ICompanyKey>() ?? false;

  protected int? CompanyKey => _currentCompany?.Id;

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<IdentityRole>().ToTable("Roles");
    modelBuilder.Entity<IdentityRole>().Property(u => u.Id).HasMaxLength(36).IsUnicode(false);
    modelBuilder.Entity<IdentityRole>().Property(u => u.ConcurrencyStamp).HasMaxLength(36);
    modelBuilder.Entity<IdentityRole>().Property(u => u.Name).HasMaxLength(100);
    modelBuilder.Entity<IdentityRole>().Property(u => u.NormalizedName).HasMaxLength(100);
    modelBuilder.Entity<IdentityRole>().HasData(SeedData.GetRoles());

    modelBuilder.Entity<IdentityRoleClaim>().ToTable("RoleClaims");
    modelBuilder.Entity<IdentityRoleClaim>().Property(u => u.RoleId).HasMaxLength(36).IsUnicode(false);
    modelBuilder.Entity<IdentityRoleClaim>().Property(u => u.ClaimType).HasMaxLength(150);
    modelBuilder.Entity<IdentityRoleClaim>().Property(u => u.ClaimValue).HasMaxLength(450);
    modelBuilder.Entity<IdentityRoleClaim>().HasData(SeedData.GetRoleClaims());

    modelBuilder.Entity<IdentityUser>().ToTable("Users");
    modelBuilder.Entity<IdentityUser>().Property(u => u.Id).HasMaxLength(36).IsUnicode(false);
    modelBuilder.Entity<IdentityUser>().Property(u => u.PasswordHash).HasMaxLength(250);
    modelBuilder.Entity<IdentityUser>().Property(u => u.SecurityStamp).HasMaxLength(36);
    modelBuilder.Entity<IdentityUser>().Property(u => u.ConcurrencyStamp).HasMaxLength(36);
    modelBuilder.Entity<IdentityUser>().Property(u => u.PhoneNumber).HasMaxLength(20).IsUnicode(false);
    modelBuilder.Entity<IdentityUser>().Property(u => u.IsDisabled).HasDefaultValue(false);

    modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
    modelBuilder.Entity<IdentityUserClaim>().Property(u => u.UserId).HasMaxLength(36).IsUnicode(false);
    modelBuilder.Entity<IdentityUserClaim>().Property(u => u.ClaimType).HasMaxLength(150);
    modelBuilder.Entity<IdentityUserClaim>().Property(u => u.ClaimValue).HasMaxLength(450);

    modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
    modelBuilder.Entity<IdentityUserRole>().Property(u => u.UserId).HasMaxLength(36).IsUnicode(false);
    modelBuilder.Entity<IdentityUserRole>().Property(u => u.RoleId).HasMaxLength(36).IsUnicode(false);

    modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
    modelBuilder.Entity<IdentityUserLogin>().Property(u => u.UserId).HasMaxLength(36).IsUnicode(false);
    modelBuilder.Entity<IdentityUserLogin>().Property(u => u.ProviderDisplayName).HasMaxLength(150);
    modelBuilder.Entity<IdentityUserLogin>().Property(u => u.ProviderKey).HasMaxLength(150);
    modelBuilder.Entity<IdentityUserLogin>().Property(u => u.LoginProvider).HasMaxLength(150);

    modelBuilder.Entity<IdentityUserToken>().ToTable("UserTokens");
    modelBuilder.Entity<IdentityUserToken>().Property(u => u.UserId).HasMaxLength(36).IsUnicode(false);
    modelBuilder.Entity<IdentityUserToken>().Property(u => u.Name).HasMaxLength(150);
    modelBuilder.Entity<IdentityUserToken>().Property(u => u.LoginProvider).HasMaxLength(150);

    var globalFiltersMethod = typeof(IdentityDbContext).GetMethod(
          nameof(ConfigureGlobalFilters),
          BindingFlags.Instance | BindingFlags.NonPublic);

    foreach (var entityType in modelBuilder.Model.GetEntityTypes())
    {
      globalFiltersMethod.MakeGenericMethod(entityType.ClrType)
        .Invoke(this, new object[] { modelBuilder, entityType });
    }
  }

  protected virtual void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
    where TEntity : class
  {
    if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
    {
      modelBuilder.Entity<TEntity>().HasQueryFilter(e => !SoftDeleteFilterEnabled || !EF.Property<bool>(e, nameof(ISoftDelete.Deleted)));
    }

    if (typeof(ICompanyKey).IsAssignableFrom(typeof(TEntity)))
    {
      modelBuilder.Entity<TEntity>().HasQueryFilter(e => !CompanyKeyFilterEnabled || EF.Property<int?>(e, nameof(ICompanyKey.CompanyId)) == CompanyKey || EF.Property<int?>(e, nameof(ICompanyKey.CompanyId)) == null);
    }
  }

  public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    foreach (var entry in ChangeTracker.Entries().ToList())
    {
      switch (entry.State)
      {
        case EntityState.Deleted:
          break;
        case EntityState.Modified:
          break;
        case EntityState.Added:
          //if (entry.Entity is ICompanyKey companyEntity)
          //{
          //  companyEntity.CompanyId = _currentCompany.Id;
          //}
          break;
        case EntityState.Detached:
          break;
        case EntityState.Unchanged:
          break;
        default:
          break;
      }
    }

    return base.SaveChangesAsync(cancellationToken);
  }
}

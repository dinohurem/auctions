using System.Reflection;
using PomoziAuctions.Core.Aggregates.BlobAggregate.Models;
using PomoziAuctions.Core.Aggregates.CompanyAggregate.Interfaces;
using PomoziAuctions.Core.Aggregates.CompanyAggregate.Models;
using PomoziAuctions.Core.Aggregates.TabAggregate.Models;
using PomoziAuctions.SharedKernel;
using PomoziAuctions.SharedKernel.DataFilters;
using PomoziAuctions.SharedKernel.Interfaces;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PomoziAuctions.Infrastructure.Data;
public class AppDbContext : DbContext, IDataProtectionKeyContext
{
  private readonly IDomainEventDispatcher? _dispatcher;
  private readonly IDataFilter _dataFilter;
  private readonly ICurrentCompany _currentCompany;

  public AppDbContext(DbContextOptions<AppDbContext> options,
    IDomainEventDispatcher dispatcher, IDataFilter dataFilter, ICurrentCompany currentCompany)
    : base(options)
  {
    _dispatcher = dispatcher;
    _dataFilter = dataFilter;
    _currentCompany = currentCompany;
  }

  public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

  protected bool SoftDeleteFilterEnabled => _dataFilter?.IsEnabled<ISoftDelete>() ?? true;

  protected bool CompanyKeyFilterEnabled => _dataFilter?.IsEnabled<ICompanyKey>() ?? false;

  protected int? CompanyKey => _currentCompany?.Id;

  public DbSet<Company> Companies => Set<Company>();

  public DbSet<Blob> Blobs { get; set; }  

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    var globalFiltersMethod = typeof(AppDbContext).GetMethod(
          nameof(ConfigureGlobalFilters),
          BindingFlags.Instance | BindingFlags.NonPublic);

    foreach (var entityType in modelBuilder.Model.GetEntityTypes())
    {
      globalFiltersMethod.MakeGenericMethod(entityType.ClrType)
        .Invoke(this, [modelBuilder, entityType]);
    }
  }

  protected virtual void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
    where TEntity : class
  {
    if (typeof(ICompanyKey).IsAssignableFrom(typeof(TEntity)) &&
      typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
    {
      modelBuilder.Entity<TEntity>().HasQueryFilter(e =>
        (!CompanyKeyFilterEnabled || EF.Property<int?>(e, nameof(ICompanyKey.CompanyId)) == CompanyKey || EF.Property<int?>(e, nameof(ICompanyKey.CompanyId)) == null) &&
        (!SoftDeleteFilterEnabled || !EF.Property<bool>(e, nameof(ISoftDelete.Deleted)))
      );
    }
    else if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
    {
      modelBuilder.Entity<TEntity>().HasQueryFilter(e => !SoftDeleteFilterEnabled || !EF.Property<bool>(e, nameof(ISoftDelete.Deleted)));
    }
    else if (typeof(ICompanyKey).IsAssignableFrom(typeof(TEntity)))
    {
      modelBuilder.Entity<TEntity>().HasQueryFilter(e => !CompanyKeyFilterEnabled || EF.Property<int?>(e, nameof(ICompanyKey.CompanyId)) == CompanyKey || EF.Property<int?>(e, nameof(ICompanyKey.CompanyId)) == null);
    }
  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
  {
    var allChanges = ChangeTracker.Entries().ToList();
    foreach (var entry in allChanges)
    {
      switch (entry.State)
      {
        case EntityState.Deleted:
          if (entry.Entity is ISoftDelete softDelete)
          {
            softDelete.Deleted = true;
            entry.State = EntityState.Modified;
          }
          break;
        case EntityState.Modified:
          break;
        case EntityState.Added:
          if (entry.Entity is ICompanyKey companyEntity && _currentCompany != null && _currentCompany.Id != null)
          {
            companyEntity.CompanyId = _currentCompany.Id;
          }
          break;
        case EntityState.Detached:
          break;
        case EntityState.Unchanged:
          break;
        default:
          break;
      }
    }

    try
    {
      int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
      // ignore events if no dispatcher provided
      if (_dispatcher == null) return result;

      // dispatch events only if save was successful
      var entitiesWithEvents = ChangeTracker.Entries<EntityBase>()
        .Select(e => e.Entity)
        .Where(e => e.DomainEvents.Any())
        .ToArray();

      await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

      return result;
    }
    catch (Exception e)
    {
      throw;
    }
  }

  public override int SaveChanges() =>
        SaveChangesAsync().GetAwaiter().GetResult();
}

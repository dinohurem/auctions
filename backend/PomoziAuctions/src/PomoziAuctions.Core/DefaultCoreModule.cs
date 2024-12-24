using Autofac;
using PomoziAuctions.Core.Aggregates.BlobAggregate.Interfaces;
using PomoziAuctions.Core.Aggregates.BlobAggregate.Services;
using PomoziAuctions.Core.Auth.Identity.Interfaces;
using PomoziAuctions.Core.Auth.Identity.Services;

namespace PomoziAuctions.Core;

public class DefaultCoreModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    // Register services below.
    builder.RegisterType<IdentityService>()
      .As<IIdentityService>().InstancePerLifetimeScope();

    builder.RegisterType<BlobService>()
      .As<IBlobService>().InstancePerLifetimeScope();
  }
}

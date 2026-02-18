using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Odyssey.Domain.Core.Extensions;
using Odyssey.GrainInterfaces.Core.Extensions;
using Odyssey.GrainInterfaces.Core.Models;
using Odyssey.Persistence.Extensions;
using Odyssey.Silo.Core.Extensions;
using Odyssey.Silo.Core.Services;

var builder = Host.CreateDefaultBuilder(args)
    .UseContentRoot(AppContext.BaseDirectory); // lets dotnet watch work correctly

builder.UseOrleans((context, silo) =>
{
    silo
        .ConfigureCluster(context.Configuration)
        .AddGrainStorage(context.Configuration, GrainConstants.GrainStorage)
        .AddStartupTask((serviceProvider, ct) =>
            serviceProvider.PerformDatabaseMigrationsAsync(ct),
            ServiceLifecycleStage.First)
        .ConfigureLogging(logging => logging.AddConsole());
    silo.ConfigureServices(services =>
    {
        services
            .AddHostedService<SiloStartupService>()
            .AddOdysseyGrainInterfacesServices(context.Configuration)
            .AddOdysseyPersistenceServerServices(context.Configuration)
            .AddOdysseyDomainServices(context.Configuration)
            .AddOdysseySiloServices(context.Configuration)
            .AddGrainStorage(context.Configuration, GrainConstants.GrainStorage);
    });
});


builder.UseConsoleLifetime();


using var host = builder.Build();
await host.RunAsync();



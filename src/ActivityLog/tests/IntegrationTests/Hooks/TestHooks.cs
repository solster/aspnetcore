using System.Threading.Channels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Solster.AspNetCore.ActivityLog.EntityFrameworkCore;

namespace Solster.AspNetCore.ActivityLog.IntegrationTests.Hooks;

[Binding]
public sealed class TestHooks(ScenarioContext ctx)
{
    private ServiceProvider? _provider;

    [BeforeScenario]
    public async Task SetupDatabase()
    {
        var dbName = Guid.NewGuid().ToString();

        var services = new ServiceCollection();
        services.AddEfCoreActivityStore(options =>
            options.UseSqlite($"DataSource='file:{dbName}?mode=memory&cache=shared'"));
        services.AddScoped<IActivityQueryService, ActivityQueryService>();
        services.AddSingleton(Channel.CreateUnbounded<ActivityLogEntry>());

        _provider = services.BuildServiceProvider();
        ctx.Set(_provider);

        var db = _provider.GetRequiredService<ActivityDbContext>();
        await db.Database.EnsureCreatedAsync();
    }

    [AfterScenario]
    public async Task TearDown()
    {
        if (_provider is not null)
        {
            await _provider.DisposeAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Solster.AspNetCore.ActivityLog.EntityFrameworkCore;

/// <summary>
/// Used only by EF Core tooling (dotnet-ef migrations). At runtime the context
/// is created via DI with the real data source supplied by AddEfCoreActivityStore.
/// </summary>
public class ActivityDbContextFactory : IDesignTimeDbContextFactory<ActivityDbContext>
{
    public ActivityDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ActivityDbContext>()
            .UseNpgsql("Host=localhost;Database=activity;Username=postgres;Password=postgres");

        return new ActivityDbContext(optionsBuilder.Options);
    }
}


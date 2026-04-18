using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Solster.AspNetCore.ActivityLog.EntityFrameworkCore;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers ActivityDbContext and EfCoreActivityStore as the IActivityStore implementation.
    /// Call this after AddActivityLogging().
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">
    ///     Configures the DbContext options — e.g. <c>opts => opts.UseNpgsql(connectionString)</c>.
    /// </param>
    public static IServiceCollection AddEfCoreActivityStore(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder> configure)
    {
        services.AddDbContext<ActivityDbContext>(configure);
        services.AddScoped<IActivityStore, EfCoreActivityStore>();
        return services;
    }

    /// <summary>
    /// Registers ActivityDbContext and EfCoreActivityStore as the IActivityStore implementation,
    /// with access to the IServiceProvider for resolving keyed or named dependencies
    /// (e.g. an Aspire-registered NpgsqlDataSource).
    /// Call this after AddActivityLogging().
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">
    ///     Configures the DbContext options with access to the DI container —
    ///     e.g. <c>(sp, opts) => opts.UseNpgsql(sp.GetRequiredKeyedService&lt;NpgsqlDataSource&gt;("activity-db"))</c>.
    /// </param>
    public static IServiceCollection AddEfCoreActivityStore(
        this IServiceCollection services,
        Action<IServiceProvider, DbContextOptionsBuilder> configure)
    {
        services.AddDbContext<ActivityDbContext>(configure);
        services.AddScoped<IActivityStore, EfCoreActivityStore>();
        return services;
    }
}


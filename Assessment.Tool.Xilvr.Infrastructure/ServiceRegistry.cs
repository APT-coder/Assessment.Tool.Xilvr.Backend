using Assessment.Tool.Xilvr.Application;
using Assessment.Tool.Xilvr.Base.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Assessment.Tool.Xilvr.Infrastructure;

/// <summary>
/// Class to register the infrastructure services.
/// </summary>
public static class ServiceRegistry
{
    private const string DB_CONNECTION = "ConnectionStrings:DbConnection";

    /// <summary>
    /// Adds the infrastructure layer services.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>(DB_CONNECTION);
        Ensure.IsNotNull(connectionString, nameof(connectionString));

        services.AddDbContext<IApplicationDbContext, ApplicationContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptionsAction: builder =>
            {
                builder.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorCodesToAdd: null);
            }).UseLoggerFactory(LoggerFactory.Create(builder => builder.AddSerilog()));
        });
    }
}

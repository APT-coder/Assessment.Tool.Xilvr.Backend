using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Assessment.Tool.Xilvr.Base.Services.Extensions;

namespace Assessment.Tool.Xilvr.Application;

/// <summary>
/// Defines the <see cref="ServiceRegistry" />.
/// </summary>
public static class ServiceRegistry
{
    /// <summary>
    /// Add application services
    /// </summary>
    /// <param name="services"></param>
    public static void AddApplicationServices(this IServiceCollection services)
    {
        var assemblies = new Assembly[] { Assembly.GetExecutingAssembly() };
        services.AddRequestHandlingServicesWithNoTransaction(assemblies);
    }
}

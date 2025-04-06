using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Reflection;

namespace Assessment.Tool.Xilvr.Base.Services.Extensions;

//
// Summary:
//     Extensions for registering the request handling Services.
public static class RequestHandlingServiceExtensions
{
    //
    // Summary:
    //     Register services for applications using EventBrokers and DB Trasactions.
    //
    // Parameters:
    //   services:
    //
    //   assemblies:
    public static void AddRequestHandlingServices(this IServiceCollection services, Assembly[] assemblies)
    {
        Assembly[] assemblies2 = assemblies;
        services.Scan(delegate (ITypeSourceSelector scan)
        {
            scan.FromAssemblies(assemblies2).AddClasses(delegate (IImplementationTypeFilter @class)
            {
                @class.AssignableTo(typeof(IValidator<>));
            }).AsImplementedInterfaces();
        });
        services.AddMediatR(delegate (MediatRServiceConfiguration cfg)
        {
            cfg.RegisterServicesFromAssemblies(assemblies2);
        });
    }

    //
    // Summary:
    //     Register services for applications using EventBrokers and No DB Transactions.
    //
    //
    // Parameters:
    //   services:
    //
    //   assemblies:
    public static void AddRequestHandlingServicesWithNoTransaction(this IServiceCollection services, Assembly[] assemblies)
    {
        Assembly[] assemblies2 = assemblies;
        services.Scan(delegate (ITypeSourceSelector scan)
        {
            scan.FromAssemblies(assemblies2).AddClasses(delegate (IImplementationTypeFilter @class)
            {
                @class.AssignableTo(typeof(IValidator<>));
            }).AsImplementedInterfaces();
        });
        services.AddMediatR(delegate (MediatRServiceConfiguration cfg)
        {
            cfg.RegisterServicesFromAssemblies(assemblies2);
        });
    }

    //
    // Summary:
    //     Register services for applications using DB transaction and No EventBrokers.
    //
    //
    // Parameters:
    //   services:
    //
    //   assemblies:
    public static void AddRequestHandlingServicesWithNoEventBroker(this IServiceCollection services, Assembly[] assemblies)
    {
        Assembly[] assemblies2 = assemblies;
        services.Scan(delegate (ITypeSourceSelector scan)
        {
            scan.FromAssemblies(assemblies2).AddClasses(delegate (IImplementationTypeFilter @class)
            {
                @class.AssignableTo(typeof(IValidator<>));
            }).AsImplementedInterfaces();
        });
        services.AddMediatR(delegate (MediatRServiceConfiguration cfg)
        {
            cfg.RegisterServicesFromAssemblies(assemblies2);
        });
    }

    //
    // Summary:
    //     Register services for applications using No DB transaction and No EventBrokers.
    //
    //
    // Parameters:
    //   services:
    //
    //   assemblies:
    public static void AddRequestHandlingServicesWithNoEventBrokerAndTransaction(this IServiceCollection services, Assembly[] assemblies)
    {
        Assembly[] assemblies2 = assemblies;
        services.Scan(delegate (ITypeSourceSelector scan)
        {
            scan.FromAssemblies(assemblies2).AddClasses(delegate (IImplementationTypeFilter @class)
            {
                @class.AssignableTo(typeof(IValidator<>));
            }).AsImplementedInterfaces();
        });
        services.AddMediatR(delegate (MediatRServiceConfiguration cfg)
        {
            cfg.RegisterServicesFromAssemblies(assemblies2);
        });
    }
}

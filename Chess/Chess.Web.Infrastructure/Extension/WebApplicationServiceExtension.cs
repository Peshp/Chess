namespace Chess.Web.Infrastructure.Extension;

using System.Reflection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static class WebApplicationServiceExtension
{
    public static void AddApplicationService(this IServiceCollection services, Type serviceType)
    {
        Assembly? serviceAssembly = Assembly.GetAssembly(serviceType);
        if (serviceAssembly == null)
            throw new InvalidOperationException("Invalid service type provided!");

        var serviceTypes = serviceAssembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && t.Name.EndsWith("Service"));

        foreach (Type implementationType in serviceTypes)
        {
            if (typeof(BackgroundService).IsAssignableFrom(implementationType))
            {
                continue;
            }

            Type? interfaceType = implementationType.GetInterface($"I{implementationType.Name}");

            if (interfaceType == null)
            {
                throw new InvalidOperationException(
                    $"No interface is provided for the service with name {implementationType.Name}. " +
                    $"Ensure I{implementationType.Name} exists or skip BackgroundServices.");
            }

            services.AddScoped(interfaceType, implementationType);
        }
    }

    public static void AddApplicationValidator(this IServiceCollection service, Type serviceType)
    {
        Assembly? serviceAssembly = Assembly.GetAssembly(serviceType);
        if (serviceAssembly == null)
        {
            throw new InvalidOperationException("Invalid validator service type provided!");
        }

        Type[] serviceTypes = serviceAssembly
            .GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface && t.GetInterface("IMoveValidator") != null)
            .ToArray();

        foreach (Type implementationType in serviceTypes)
        {
            Type? interfaceType = implementationType
                .GetInterface($"IMoveValidator");
            if (interfaceType == null)
            {
                throw new InvalidOperationException(
                    $"No interface is provided for the service with name {implementationType.Name}");
            }

            service.AddScoped(interfaceType, implementationType);
        }
    }
}  

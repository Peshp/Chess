namespace Chess.Web.Infrastructure.Extension;

using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

public static class WebApplicationServiceExtension
{
    public static void AddApplicationService(this IServiceCollection service, Type serviceType)
    {
        Assembly? serviceAssembly = Assembly.GetAssembly(serviceType);
        if (serviceAssembly == null)
        {
            throw new InvalidOperationException("Invalid service type provided!");
        }

        Type[] serviceTypes = serviceAssembly
            .GetTypes()
            .Where(t => t.Name.EndsWith("Service") && !t.IsInterface)
            .ToArray();

        foreach (Type implementationType in serviceTypes)
        {
            Type? interfaceType = implementationType
                .GetInterface($"I{implementationType.Name}");
            if (interfaceType == null)
            {
                throw new InvalidOperationException(
                    $"No interface is provided for the service with name {implementationType.Name}");
            }
            service.AddScoped(interfaceType, implementationType);
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

namespace Contracts.Common
{
    using Contracts.Events;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventHandlers(this IServiceCollection services, Assembly assembly)
        {
            var handlerInterfaceType = typeof(IEventHandler<>);
            Type[] allTypes = assembly.GetTypes();

            var handlers = allTypes
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType)
                    .Select(i => new { Interface = i, Implementation = t }));

            foreach (var handler in handlers)
            {
                services.AddTransient(handler.Interface, handler.Implementation);
            }

            return services;
        }
    }


}

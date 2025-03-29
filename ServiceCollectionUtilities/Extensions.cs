using Microsoft.Extensions.DependencyInjection;

namespace ServiceCollectionUtilities
{
    public static class Extensions
    {
        /// <summary>
        /// Adds a factory for the specified service type to the IServiceCollection.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="services">The IServiceCollection to add the factory to.</param>
        /// <remarks>This method lets you easily create a factory class</remarks>
        public static void AddFactory<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            services.AddTransient<TService, TImplementation>();
            services.AddSingleton<Func<TService>>(x => () => x.GetService<TService>());
            services.AddSingleton<IFactory<TService>, Factory<TService>>();
        }

        /// <summary>
        /// Adds a factory for the specified service type to the IServiceCollection.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="services">The IServiceCollection to add the factory to.</param>
        /// <remarks>This method lets you easily create a factory class</remarks>
        public static void AddFactory<TImplementation>(this IServiceCollection services)
	        where TImplementation : class
        {
	        services.AddTransient<TImplementation>();
	        services.AddSingleton<Func<TImplementation>>(x => () => x.GetService<TImplementation>());
	        services.AddSingleton<IFactory<TImplementation>, Factory<TImplementation>>();
        }
	}
}

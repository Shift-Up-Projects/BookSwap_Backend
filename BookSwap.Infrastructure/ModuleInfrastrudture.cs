using BookSwap.Infrastructure.InfrastructureBases;
using Microsoft.Extensions.DependencyInjection;

namespace BookSwap.Infrastructure
{
    public static class ModuleInfrastrudtureDepndencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));

            return services;
        }

    }
}

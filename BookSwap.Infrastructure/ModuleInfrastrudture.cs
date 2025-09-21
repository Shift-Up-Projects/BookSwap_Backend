using BookSwap.Infrastructure.Abstracts;
using BookSwap.Infrastructure.InfrastructureBases;
using BookSwap.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BookSwap.Infrastructure
{
    public static class ModuleInfrastrudtureDepndencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddTransient<IRefreshTokenRepositoryAsync, RefreshTokenRepositoryAsync>();

            return services;
        }

    }
}

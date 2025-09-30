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
            services.AddScoped(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddScoped<IRefreshTokenRepositoryAsync, RefreshTokenRepositoryAsync>();
            services.AddScoped<IBookRepositoryAsync, BookRepositoryAsync>();
            services.AddScoped<ICategoryRepositoryAsync, CategoryRepositoryAsync>();
            services.AddScoped<IExchangeOfferRepositoryAsync, ExchangeOfferRepositoryAsync>();
            services.AddScoped<IUserRepositoryAsync, UserRepositoryAsync>();
            services.AddScoped<IOfferedBookRepositoryAsync, OfferedBookRepositoryAsync>();
            services.AddScoped<IBookOwnershipHistoryRepositoryAsync, BookOwnershipHistoryRepositoryAsync>();
            return services;
        }

    }
}

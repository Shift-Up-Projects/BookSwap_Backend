using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookSwap.Application
{
    public static class ModuleApiServicesDependencies
    {
        public static IServiceCollection AddApiServicesDependencies(this IServiceCollection services, IConfiguration configuration)
        {          
            return services;
        }
    }
}

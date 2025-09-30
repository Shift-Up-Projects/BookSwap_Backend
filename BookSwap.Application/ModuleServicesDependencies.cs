

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookSwap.Core.Helping;
using BookSwap.Application.Abstracts;
using BookSwapImplementations;
using BookSwap.Application.Implementations;
using BookSwap.Application.Implemetations;
using BookExchange.Infrastructure.Services;
namespace BookSwap.Application
{
    public static class ModuleApiServicesDependencies
    {
        public static IServiceCollection AddServicesDependencies(this IServiceCollection services, IConfiguration configuration)
        {      
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<IAuthonticationService, AuthonticationService>();
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IExchangeOfferService, ExchangeOfferService>();


            //BackgroundServices
            services.AddHostedService<RefreshTokenCleanupService>();

            //Email Setting
            var emailSettings = new EmailSettings();
            configuration.GetSection(nameof(emailSettings)).Bind(emailSettings);
            services.AddSingleton(emailSettings);

            //JWT Authentication
            var jwtSettings = new JwtSettings();
            configuration.GetSection(nameof(jwtSettings)).Bind(jwtSettings);
            services.AddSingleton(jwtSettings);

       


            return services;
        }
    }
}


using BookSwap.Api.Behavior;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using BookSwap.Application;
using Microsoft.EntityFrameworkCore;
using BookSwap.Infrastructure.Context;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BookSwap.Core.Entities.Identity;
using BookSwap.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BookExchange.Infrastructure.Services;

namespace BookSwap.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(op => op.Filters.Add(typeof(ValidationFilter)))
                  .ConfigureApiBehaviorOptions(options => options.InvalidModelStateResponseFactory = context => null!)
                  .AddJsonOptions(options =>
                  {
                      options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                      options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                  });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.MapType<TimeSpan>(() => new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("00:00:00")
                });
            });

            builder.Services.AddDbContext<BookSwapDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddServicesDependencies(builder.Configuration)
                            .AddInfrastructureDependencies();

            //builder.Services.AddFluentValidationAutoValidation()
            //                    .AddValidatorsFromAssembly();

            builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            builder.Services.AddTransient<IUrlHelper>(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext!);
            });

            #region addIdentity

            builder.Services.AddIdentity<User, Role>(option =>
            {
                // Password settings.
                option.Password.RequireDigit = true;
                option.Password.RequireLowercase = true;
                option.Password.RequireNonAlphanumeric = true;
                option.Password.RequireUppercase = true;
                option.Password.RequiredLength = 6;
                option.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                option.Lockout.MaxFailedAccessAttempts = 5;
                option.Lockout.AllowedForNewUsers = true;

                // User settings.
                option.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                option.User.RequireUniqueEmail = true;
                option.SignIn.RequireConfirmedEmail = true;


            }).AddEntityFrameworkStores<BookSwapDbContext>().AddDefaultTokenProviders();
            #endregion
            #region Authontication
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = Boolean.Parse(builder.Configuration["jwtSettings:ValidateIssuer"]!),
                    ValidIssuers = new[] { builder.Configuration["jwtSettings:Issuer"] },
                    ValidateIssuerSigningKey = Boolean.Parse(builder.Configuration["jwtSettings:ValidateIssuerSigningKey"]!),

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwtSettings:Secret"]!)),
                    ValidAudience = builder.Configuration["jwtSettings:Audience"],
                    ValidateAudience = Boolean.Parse(builder.Configuration["jwtSettings:ValidateAudience"]!),
                    ValidateLifetime = Boolean.Parse(builder.Configuration["jwtSettings:ValidateLifeTime"]!),
                };
            });

            #endregion
            #region CORS
            var allowReactApp = "AllowReactApp";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(allowReactApp,
                    policy =>
                    {
                        policy.AllowAnyOrigin() 
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                       
                    });
            });
            #endregion
            #region Swagger Gn
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Book Swab", Version = "v1" });
                // c.EnableAnnotations();

                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = JwtBearerDefaults.AuthenticationScheme
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
            });
            #endregion

            //Auth Filter
            builder.Services.AddTransient<AuthFilter>();
            builder.Services.AddHostedService<RefreshTokenCleanupService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

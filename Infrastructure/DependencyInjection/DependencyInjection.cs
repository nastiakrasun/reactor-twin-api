using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ReactorTwinAPI.Infrastructure.Persistence;
using ReactorTwinAPI.Features.ReactorTwins.Repositories;
using ReactorTwinAPI.Features.ReactorTwins.Services;
using ReactorTwinAPI.Features.Users.Repositories;
using ReactorTwinAPI.Application.Services;

namespace ReactorTwinAPI.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Repositories & Services
            services.AddScoped<IReactorTwinRepository, ReactorTwinRepository>();
            services.AddScoped<IReactorTwinService, ReactorTwinService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<Features.Auth.Services.IAuthService, Features.Auth.Services.AuthService>();

            // AutoMapper (scan profiles in ReactorTwins and Users)
            services.AddAutoMapper(typeof(Features.ReactorTwins.Mapping.ReactorTwinProfile).Assembly,
                                   typeof(Features.Users.Mapping.UserProfile).Assembly);

            // HttpContext + current user helper
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            // Authentication (JWT)
            var jwtKey = configuration["Jwt:Key"];
            if (!string.IsNullOrEmpty(jwtKey))
            {
                var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                        ValidateIssuer = !string.IsNullOrEmpty(configuration["Jwt:Issuer"]),
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidateAudience = !string.IsNullOrEmpty(configuration["Jwt:Audience"]),
                        ValidAudience = configuration["Jwt:Audience"],
                        ValidateLifetime = true
                    };
                });

                services.AddAuthorization();
            }

            return services;
        }
    }
}

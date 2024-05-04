using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentConnect.API.src.Data;
using StudentConnect.API.src.Interface;
using StudentConnect.API.src.Middlewares;
using StudentConnect.API.src.Models.Data;
using StudentConnect.API.src.Repositories;
using StudentConnect.API.src.Services;
using StudentConnect.API.src.Settings;
using System.Text;

namespace StudentConnect.API.src.Extensions
{
    public static class ServiceExtensions
    {
        // Constants for configuration sections
        private const string ConnectionStringName = "ConnectionString";
        private const string JwtOptionsSectionName = "JwtOptions";

        // Method to configure the database context with a connection string from configuration
        public static IServiceCollection ConfigureMsSqlServer(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString(ConnectionStringName);
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            return services;
        }

        // Method to configure business services for dependency injection
        public static IServiceCollection ConfigureBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        // Method to append the global error handler middleware to the pipeline
        public static void AppendGlobalErrorHandler(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<GlobalErrorHandler>();
        }

        // Method to configure JWT authentication
        public static IServiceCollection ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var jwtSection = config.GetSection(JwtOptionsSectionName);
            services.Configure<JwtOptions>(jwtSection);
            var jwtOptions = jwtSection.Get<JwtOptions>();

            services
               .AddAuthentication(options =>
               {
                   options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                   options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                   options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
               })
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidIssuer = jwtOptions.Issuer,
                       ValidateAudience = true,
                       ValidAudience = jwtOptions.Audience,
                       ValidateLifetime = true,
                       IssuerSigningKey = new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(jwtOptions.Secret)
                       ),
                       ValidateIssuerSigningKey = true
                   };
               });
            return services;
        }

        // Method to configure Identity
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services, IConfiguration config)
        {
            services
                .AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 1;
            });
            return services;
        }
    }


}

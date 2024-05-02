using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentConnect.API.src.Data;
using StudentConnect.API.src.Middlewares;
using StudentConnect.API.src.Models.Data;
using StudentConnect.API.src.Settings;
using System.Text;

namespace StudentConnect.API.src.Extensions
{
    public static class ServiceExtensions
    {



        // method to connect to the database 
        public static IServiceCollection ConfigureMsSqlServer(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("ConnectionString");
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            return services;
        }


        public static IServiceCollection ConfigureBusinessServices(this IServiceCollection services) {
            // Todo ging to add servicde and Respository here 
        
        }


        // Reister error handle here 

        public static void AppendGlobalErrorHandler(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<GlobalErrorHandler>();
        }


        public static IServiceCollection ConfigureBusinessServices(this IServiceCollection services, IConfiguration config)
        {
            var jwtSection = config.GetSection("JwtOptions");
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

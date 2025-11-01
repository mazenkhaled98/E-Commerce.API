using Domain.Contracts;
using Domain.Entites.IdentityModule;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Presistence.Data;
using Presistence.Data.Identity;
using Presistence.Repositories;
using Shared.Common;
using StackExchange.Redis;
using System.Text;

namespace E_Commerce.API.Extensions
{
    public static class InfraStructureServicesExtension
    {
        public static IServiceCollection AddInfraStructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            });
           
            services.AddDbContext<IdentityStoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });

            services.AddScoped<IDataSeeding, DataSeeding>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            
            services.AddSingleton<IConnectionMultiplexer>((_) =>
            {
               return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!);
            });


            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<IdentityStoreDbContext>();

            services.AddScoped<IBasketRepository, BasketRepository>();
            services.ValidateJwt(configuration);
            return services;
        }


        public static IServiceCollection ValidateJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtoptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
            ).AddJwtBearer(options => {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer=true,
                    ValidateAudience=true,
                    ValidateLifetime=true,
                    ValidateIssuerSigningKey=true,
                    ValidIssuer=jwtoptions.Issuer,
                    ValidAudience=jwtoptions.Audience,
                    IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtoptions.SecretKey))
                };
            });
            services.AddAuthorization();
            return services;
        }
    }
}

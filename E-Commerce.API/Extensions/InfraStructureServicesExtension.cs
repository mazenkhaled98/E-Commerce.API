using Domain.Contracts;
using Domain.Entites.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Presistence.Data;
using Presistence.Data.Identity;
using Presistence.Repositories;
using StackExchange.Redis;

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
            services.AddScoped<IBasketRepository, BasketRepository>();
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

            return services;
        }
    }
}

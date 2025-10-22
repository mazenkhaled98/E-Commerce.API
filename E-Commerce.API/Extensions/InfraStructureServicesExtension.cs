using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Presistence.Data;
using Presistence.Repositories;

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

            services.AddScoped<IDataSeeding, DataSeeding>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}

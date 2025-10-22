using Services;
using Services.Abstraction.Contracts;
using Services.Implementations;


namespace E_Commerce.API.Extensions
{
    public static class CoreServicesExtension
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
           services.AddAutoMapper(cfg => { }, typeof(AssemblyReference).Assembly);
            services.AddScoped<IServiceManager, ServiceManager>();
            return services;
        }
    }
}

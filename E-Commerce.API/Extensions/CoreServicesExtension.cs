using Services;
using Services.Abstraction.Contracts;
using Services.Contracts;
using Services.Implementaitons;
using Services.Implementations;
using Shared.Common;


namespace E_Commerce.API.Extensions
{
    public static class CoreServicesExtension
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services , IConfiguration configuration)
        {
           services.AddAutoMapper(cfg => { }, typeof(AssemblyReference).Assembly);
            services.AddScoped<IServiceManager, ServiceManagerWithFactoryDelegate>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<ICasheService, CasheService>();

            services.AddScoped<Func<IProductService>>(provider =>
                () => provider.GetRequiredService<IProductService>()
            );
            services.AddScoped<Func<IAuthenticationService>>(provider =>
                () => provider.GetRequiredService<IAuthenticationService>()
            );
            services.AddScoped<Func<IBasketService>>(provider =>
                () => provider.GetRequiredService<IBasketService>()
            );
            services.AddScoped<Func<IOrderService>>(provider =>
                () => provider.GetRequiredService<IOrderService>()
            );
            services.AddScoped<Func<IPaymentService>>(provider =>
                () => provider.GetRequiredService<PaymentService>()
            );
            services.AddScoped<Func<ICasheService>>(provider =>
                () => provider.GetRequiredService<ICasheService>()
            );
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
            return services;
        }
    }
}

using E_Commerce.API.Factories;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Extensions
{
    public static class WebApiServicesExtension
    {
        public static IServiceCollection AddWebApiServices(this IServiceCollection services,IConfiguration configuration)
        {

            services.AddControllers();
            services.AddCors(options =>
            {
                //url of the project of the angular
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod()
                    .WithOrigins(configuration.GetSection("URLS")["FrontUrl"]);
                });

            });
            
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponceFactory.CustomValidationErrorResponse;
            });

            return services;
        }
    }
}

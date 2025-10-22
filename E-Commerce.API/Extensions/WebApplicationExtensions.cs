using Domain.Contracts;
using E_Commerce.API.Middlewares;

namespace E_Commerce.API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task<WebApplication> SeedDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dataSeeding = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
            await dataSeeding.SeedDataAsync();

            return app;
        }

        public static WebApplication UseExceptionHandlingMiddlewares(this WebApplication app)
        {
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
            return app;
        }

        public static WebApplication UseSwaggerMiddlewares(this WebApplication app)
        {
            app.UseSwagger(); // miiddleware of swagger
            app.UseSwaggerUI(); //middleware of swagger UI
            return app;
        }
    }
}

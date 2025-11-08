
using Domain.Contracts;
using E_Commerce.API.Extensions;
using E_Commerce.API.Factories;
using E_Commerce.API.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presistence.Data;
using Presistence.Repositories;
using Services;
using Services.Abstraction.Contracts;
using Services.Implementations;

namespace E_Commerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region DI Container
            //web api services extension method
            builder.Services.AddWebApiServices(builder.Configuration);


            // infrastructure services extension method
            builder.Services.AddInfraStructureServices(builder.Configuration);


            //core services extension method
            builder.Services.AddCoreServices(builder.Configuration);
            #endregion


            #region Piplines - Middlewares
            var app = builder.Build();

            //seed database
            await app.SeedDatabaseAsync();


            //middleware for exception handling can be added here
            app.UseExceptionHandlingMiddlewares();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();
            }
         

          

            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

             app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

             app.Run(); 
            #endregion
        }
    }
}

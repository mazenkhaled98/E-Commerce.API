using Domain.Contracts;
using Domain.Entites.IdentityModule;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace Presistence.Data
{
    public class DataSeeding(StoreDbContext _dbContext , RoleManager<IdentityRole> _role ,UserManager<User> _user) : IDataSeeding
    {
        public async Task SeedDataAsync()
        {
            //any pending migrations, apply them to database
            try
            {
                var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                   await _dbContext.Database.MigrateAsync();
                }



                //seed data if no data exists

                if (!_dbContext.ProductBrands.Any())
                {
                    var brandsData = File.OpenRead("../Infrastructure/Presistence/Data/DataSeed/brands.json");
                    var brands =await JsonSerializer.DeserializeAsync<List<ProductBrand>>(brandsData);
                    if (brands != null && brands.Any())
                    {
                        await _dbContext.ProductBrands.AddRangeAsync(brands);
                    }
                }


                if (!_dbContext.ProductTypes.Any())
                {
                    var typesData = File.OpenRead("../Infrastructure/Presistence/Data/DataSeed/types.json");
                    var types = await JsonSerializer.DeserializeAsync<List<ProductType>>(typesData);
                    if (types != null && types.Any())
                    {
                      await  _dbContext.ProductTypes.AddRangeAsync(types);
                    }
                }


                if (!_dbContext.Products.Any())
                {
                    var productsData = File.OpenRead("../Infrastructure/Presistence/Data/DataSeed/products.json");
                    var products =await JsonSerializer.DeserializeAsync<List<Product>>(productsData);
                    if (products != null && products.Any())
                    {
                       await _dbContext.Products.AddRangeAsync(products);
                    }

                }
               await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                //handle exception
            }


        }

        public async Task SeedIdentityDataAsync()
        {
            try
            {

                //seed roles [admin, superadmin]
                //seed users [adminuser, superadminuser]]
                //assign roles to users
                if (!_role.Roles.Any())
                {
                    await _role.CreateAsync(new IdentityRole("Admin"));
                    await _role.CreateAsync(new IdentityRole("SuperAdmin"));

                }
                if (!_user.Users.Any())
                {
                    var adminUser = new User
                    {
                        DisplayName = "Admin",
                        UserName = "adminuser",
                        Email = "Admin@gmail.com",
                        PhoneNumber = "1234567890",
                    };
                    var superAdminUser = new User
                    {
                        DisplayName = "SuperAdmin",
                        UserName = "superadminuser",
                        Email = "SuperAdmin@gmail.com",
                        PhoneNumber = "0987654321",
                    };
                    await _user.CreateAsync(adminUser, "P@ssw0rd1");
                    await _user.CreateAsync(superAdminUser, "P@ssw0rd1");
                    await _user.AddToRoleAsync(adminUser, "Admin");
                    await _user.AddToRoleAsync(superAdminUser, "SuperAdmin");

                }

                
            }
            catch (Exception ex)
            {
                //handle exception
                throw;
            }
        }
    }
}

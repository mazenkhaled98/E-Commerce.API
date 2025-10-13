using Domain.Contracts;
using System.Text.Json;

namespace Presistence.Data
{
    public class DataSeeding(StoreDbContext _dbContext) : IDataSeeding
    {
        public void SeedData()
        {
            //any pending migrations, apply them
            try
            {
                if (_dbContext.Database.GetPendingMigrations().Any())
                {
                    _dbContext.Database.Migrate();
                }



                //seed data if no data exists

                if (!_dbContext.ProductBrands.Any())
                {
                    var brandsData = File.ReadAllText("../Infrastructure/Presistence/Data/DataSeed/brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    if (brands != null && brands.Any())
                    {
                        _dbContext.ProductBrands.AddRange(brands);
                    }
                }


                if (!_dbContext.ProductTypes.Any())
                {
                    var typesData = File.ReadAllText("../Infrastructure/Presistence/Data/DataSeed/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    if (types != null && types.Any())
                    {
                        _dbContext.ProductTypes.AddRange(types);
                    }
                }


                if (!_dbContext.Products.Any())
                {
                    var productsData = File.ReadAllText("../Infrastructure/Presistence/Data/DataSeed/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    if (products != null && products.Any())
                    {
                        _dbContext.Products.AddRange(products);
                    }

                }
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                //handle exception
            }


        }
    }
}

using EyeCareHub.DAL.Entities.ProductInfo;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EyeCareHub.DAL.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            try
            {
                if (!context.ProductBrands.Any())
                {
                    var productBrand = File.ReadAllText("../EyeCareHub.DAL/Data/DataSeed/brands.json"); //EyeCareHub.DAL
                    var Brands = JsonSerializer.Deserialize<List<ProductBrands>>(productBrand);
                    foreach (var Brand in Brands)
                    {
                        await context.Set<ProductBrands>().AddAsync(Brand);
                    }
                    context.SaveChanges();
                }

                if (!context.ProductTypes.Any())
                {
                    var productType = File.ReadAllText("../EyeCareHub.DAL/Data/DataSeed//types.json");
                    var Types = JsonSerializer.Deserialize<List<ProductTypes>>(productType);
                    foreach (var Type in Types)
                    {
                        await context.Set<ProductTypes>().AddAsync(Type);
                    }
                    context.SaveChanges();
                }

                if (!context.ProductCategories.Any())
                {
                    var category = File.ReadAllText("../EyeCareHub.DAL/Data/DataSeed//categories.json");
                    var categorys = JsonSerializer.Deserialize<List<ProductCategory>>(category);
                    foreach (var item in categorys)
                    {
                        await context.Set<ProductCategory>().AddAsync(item);
                    }
                    context.SaveChanges();
                }

                if (!context.Products.Any())
                {
                    var products = File.ReadAllText("../EyeCareHub.DAL/Data/DataSeed//products.json");
                    var Product = JsonSerializer.Deserialize<List<Products>>(products);
                    foreach (var product in Product)
                    {
                        await context.Set<Products>().AddAsync(product);
                    }
                    context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                throw;
                //LoggerFactory.LogError(ex, "An error occurred while seeding the database.");
            }
        }
    }
}

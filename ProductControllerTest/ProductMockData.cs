using Microsoft.Extensions.DependencyInjection;
using ProductAPI.Data;
using ProductAPI.Models;

namespace ProductAPI.Tests
{
    internal class ProductMockData
    {
        public static async Task CreateProducts(ProductAPIApplication application, bool criar)
        {
            using (var scope = application.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                using (var productDbContext = provider.GetRequiredService<DataContext>())
                {
                    await productDbContext.Database.EnsureCreatedAsync();

                    if (criar)
                    {
                        await productDbContext.Products.AddAsync(new Product { Id = Guid.NewGuid(), Name = "Produto 1", Stock = 50, Price = 50.2m  });
                        await productDbContext.Products.AddAsync(new Product { Id = Guid.NewGuid(), Name = "Produto 2", Stock = 30, Price = 25.1m });
                        await productDbContext.SaveChangesAsync();
                    }
                }
            }
        }
    }
}

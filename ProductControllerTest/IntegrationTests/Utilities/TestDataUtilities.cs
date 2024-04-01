using Microsoft.Extensions.DependencyInjection;
using ProductAPI.Data;
using ProductAPI.Models;

namespace ProductAPI.Tests.IntegrationTests.Utilities
{
    public static class TestDataUtilities
    {
        public static async Task CreateProductInDatabase(ProductAPIApplication application, Product product)
        {
            using var scope = application.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var dbContext = serviceProvider.GetRequiredService<DataContext>();

            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
        }
    }
}

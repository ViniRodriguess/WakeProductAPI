using Microsoft.AspNetCore.Mvc.Testing;
using ProductAPI.Models;
using System.Net;
using System.Net.Http.Json;

using ProductAPI.Tests.IntegrationTests.Utilities;

namespace ProductAPI.Tests
{
    public class ApiProductIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ApiProductIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        public void Dispose()
        {
            _factory.Dispose();
        }

        [Fact]
        public async Task GET_Retorna_Todos_Produtos()
        {
            using var application = new ProductAPIApplication();

            await ProductMockData.CreateProducts(application, true);
            var url = "/api/Product";

            var client = application.CreateClient();

            var result = await client.GetAsync(url);
            var products = await client.GetFromJsonAsync<List<Product>>("/api/Product");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(products);
            Assert.Equal(2, products.Count);
        }

        [Fact]
        public async Task GET_Retorna_Produtos_Vazio()
        {
            using var application = new ProductAPIApplication();

            await ProductMockData.CreateProducts(application, false);
            var url = "/api/Product";

            var client = application.CreateClient();
            var products = await client.GetFromJsonAsync<List<Product>>(url);

            Assert.NotNull(products);
            Assert.Empty(products);
        }

        [Fact]
        public async Task DELETE_Exclui_Produto_Existente()
        {
            using var application = new ProductAPIApplication();

            await ProductMockData.CreateProducts(application, true);

            var client = application.CreateClient();
            var products = await client.GetFromJsonAsync<List<Product>>("/api/Product");

            Assert.NotNull(products);

            if (products.Count > 0)
            {
                var productToDelete = products[0];
                var url = $"/api/Product/{productToDelete.Id}";

                var response = await client.DeleteAsync(url);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
            else
            {
                Assert.True(true, "Não há produtos disponíveis para exclusão.");
            }
        }
            [Fact]
        public async Task DELETE_Exclui_Produto_Inexistente()
        {
            using var application = new ProductAPIApplication();

            await ProductMockData.CreateProducts(application, true);

            var url = "/api/Product/999";

            var client = application.CreateClient();
            var response = await client.DeleteAsync(url);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Controllers;
using ProductAPI.Data;
using ProductAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;

namespace ProductAPI.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public async Task GetAllProducts_ReturnsOkResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new DataContext(options);
            var controller = new ProductController(context);

            // Act
            var result = await controller.GetAllProducts();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetAllProducts_ReturnsEmptyList()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new DataContext(options);
            var controller = new ProductController(context);

            // Act
            var result = await controller.GetAllProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var products = Assert.IsAssignableFrom<List<Product>>(okResult.Value);
            Assert.Empty(products);
        }


        [Fact]
        public async Task GetOrderedProducts_ReturnsOkResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new DataContext(options);
            var controller = new ProductController(context);

            // Act
            var result = await controller.GetOrderedProducts("name");

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetOrderedProducts_ReturnsBadRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Criar um contexto de banco de dados simulado usando o contexto de banco de dados real
            var context = new DataContext(options);

            // Simular um contexto de controlador com o contexto de banco de dados simulado
            var controller = new ProductController(context);

            // Act
            var result = await controller.GetOrderedProducts("InvalidSortBy");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result); // Verificar se a resposta é BadRequest
            Assert.Equal("Campo de ordenação inválido.", badRequestResult.Value); // Verificar a mensagem de erro retornada
        }


        [Fact]
        public async Task SearchProducts_ReturnsOkResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new DataContext(options);
            var controller = new ProductController(context);

            // Adicione alguns produtos de teste ao banco de dados em memória
            var product1 = new Product { Name = "Example Product 1", Stock = 10, Price = 20 };
            var product2 = new Product { Name = "Example Product 2", Stock = 15, Price = 25 };
            await context.Products.AddRangeAsync(product1, product2);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.SearchProducts("Example Product 1"); // Pesquise por um produto existente

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task SearchProducts_ReturnsNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Criar um contexto de banco de dados simulado usando o contexto de banco de dados real
            var context = new DataContext(options);

            // Simular um contexto de controlador com o contexto de banco de dados simulado
            var controller = new ProductController(context);

            // Act
            var result = await controller.SearchProducts("Nonexistent Product");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result); // Verificar se a resposta é NotFound
            Assert.Equal("Nenhum produto encontrado com esse nome.", notFoundResult.Value); // Verificar a mensagem de erro retornada
        }


        [Fact]
        public async Task GetProduct_ReturnsOkWithProduct()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Criar um contexto de banco de dados simulado usando o contexto de banco de dados real
            var context = new DataContext(options);

            // Adicionar um produto com um ID específico ao banco de dados
            var productId = Guid.NewGuid();
            var productName = "Test Product";
            var product = new Product { Id = productId, Name = productName };
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            // Simular um contexto de controlador com o contexto de banco de dados simulado
            var controller = new ProductController(context);

            // Act
            var result = await controller.GetProduct(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result); // Verificar se a resposta é Ok
            var returnedProduct = Assert.IsType<Product>(okResult.Value); // Verificar se o valor retornado é um produto
            Assert.Equal(productName, returnedProduct.Name); // Verificar se o nome do produto retornado é o mesmo que o nome do produto adicionado
        }


        [Fact]
        public async Task GetProduct_ReturnsNotFoundResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new DataContext(options);
            var controller = new ProductController(context);

            // Act
            var result = await controller.GetProduct(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }


        [Fact]
        public async Task AddProduct_ReturnsOkResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new DataContext(options);
            var controller = new ProductController(context);

            // Act
            var result = await controller.AddProduct("Product Test", 10, 50); // Chama o método AddProduct com parâmetros válidos

            // Assert
            Assert.IsType<OkObjectResult>(result.Result); // Verifica se o resultado retornado é um OkObjectResult
        }


        [Fact]
        public async Task AddProduct_ReturnsBadRequestForNegativePrice()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new DataContext(options);
            var controller = new ProductController(context);

            // Act
            var result = await controller.AddProduct("Example", 10, -50); // Tenta adicionar um produto com preço negativo

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }



        [Fact]
        public async Task UpdateProduct_ReturnsOkResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new DataContext(options);
            var controller = new ProductController(context);

            // Adicione um produto ao banco de dados de teste
            var newProduct = new Product { Name = "Example", Stock = 10, Price = 50 };
            await context.Products.AddAsync(newProduct);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.UpdateProduct(newProduct.Id, "Updated Example", 20, 100); // Atualize o produto adicionado

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsBadRequestForNegativePrice()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new DataContext(options);
            var controller = new ProductController(context);

            // Adicione um produto de teste para atualização
            var product = new Product { Id = Guid.NewGuid(), Name = "Product", Stock = 10, Price = 50 };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.UpdateProduct(product.Id, "Updated Product", 20, -50); // Tenta atualizar o produto com preço negativo

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }



        [Fact]
        public async Task DeleteProduct_ReturnsOkResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var context = new DataContext(options);
            var controller = new ProductController(context);

            // Crie um novo produto no banco de dados
            var newProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product Name",
                Stock = 10,
                Price = 100
            };
            context.Products.Add(newProduct);
            await context.SaveChangesAsync();

            // Act
            var result = await controller.DeleteProduct(newProduct.Id);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }


        [Fact]
        public async Task DeleteProduct_ReturnsNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Criar um contexto de banco de dados simulado usando o contexto de banco de dados real
            var context = new DataContext(options);

            // Adicionar um produto com um ID específico ao banco de dados (neste caso, não adicionaremos nenhum produto)

            // Simular um contexto de controlador com o contexto de banco de dados simulado
            var controller = new ProductController(context);

            // Act
            var result = await controller.DeleteProduct(Guid.NewGuid()); // Tentar excluir um produto que não existe

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result); // Verificar se a resposta é NotFound
        }






    }
}

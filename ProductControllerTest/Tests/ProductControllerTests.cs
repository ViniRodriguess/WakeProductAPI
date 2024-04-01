using Microsoft.AspNetCore.Mvc;
using ProductAPI.Controllers;
using ProductAPI.Models;
using ProductAPI.Repository.Interfaces;
using Moq;
using System.Linq.Expressions;
using System.Linq;

namespace ProductAPI.Tests
{
    public class ProductControllerTests
    {
        private readonly Mock<IBaseRepository<Product>> _mockRepository;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mockRepository = new Mock<IBaseRepository<Product>>();
            _controller = new ProductController(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllProducts_ReturnsOkResultWithListOfProducts()
        {
            // Arrange
            var expectedProducts = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Product A", Stock = 10, Price = 20.0m },
                new Product { Id = Guid.NewGuid(), Name = "Product B", Stock = 15, Price = 25.0m }
            };

            var productRepositoryMock = new Mock<IBaseRepository<Product>>();
            productRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(expectedProducts);

            var controller = new ProductController(productRepositoryMock.Object);

            // Act
            var result = await controller.GetAllProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProducts = Assert.IsAssignableFrom<List<Product>>(okResult.Value);
            Assert.Equal(expectedProducts.Count, returnedProducts.Count);
        }

        [Fact]
        public async Task GetAllProducts_ReturnsEmptyList()
        {
            // Arrange
            var expectedProducts = new List<Product>();

            var productRepositoryMock = new Mock<IBaseRepository<Product>>();
            productRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(expectedProducts);

            var controller = new ProductController(productRepositoryMock.Object);

            // Act
            var result = await controller.GetAllProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProducts = Assert.IsAssignableFrom<List<Product>>(okResult.Value);
            Assert.Empty(returnedProducts);
        }

        [Fact]
        public async Task GetOrderedProducts_ReturnsOkResultWithListOfOrderedProducts()
        {
            // Arrange
            var sortBy = "name";
            var expectedProducts = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Product B", Stock = 15, Price = 25.0m },
                new Product { Id = Guid.NewGuid(), Name = "Product A", Stock = 10, Price = 20.0m }
            };

            var productRepositoryMock = new Mock<IBaseRepository<Product>>();
            productRepositoryMock.Setup(repo => repo.GetAllOrderedBy(It.IsAny<Expression<Func<Product, object>>>()))
                                 .ReturnsAsync(expectedProducts);

            var controller = new ProductController(productRepositoryMock.Object);

            // Act
            var result = await controller.GetOrderedProducts(sortBy);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProducts = Assert.IsAssignableFrom<List<Product>>(okResult.Value);
            Assert.Equal(expectedProducts.Count, returnedProducts.Count);
        }

        [Fact]
        public async Task GetOrderedProducts_ReturnsBadRequest()
        {
            // Arrange
            var sortBy = "invalid";
            var controller = new ProductController(Mock.Of<IBaseRepository<Product>>());

            // Act
            var result = await controller.GetOrderedProducts(sortBy);

            // Assert
            var statusCodeResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task SearchProducts_ReturnsOkResultWithMatchingProducts()
        {
            // Arrange
            var searchName = "Product";
            var expectedProducts = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Product A", Stock = 10, Price = 20.0m },
                new Product { Id = Guid.NewGuid(), Name = "Product B", Stock = 15, Price = 25.0m }
            };

            var productRepositoryMock = new Mock<IBaseRepository<Product>>();
            productRepositoryMock.Setup(repo => repo.SearchByName(searchName)).ReturnsAsync(expectedProducts);

            var controller = new ProductController(productRepositoryMock.Object);

            // Act
            var result = await controller.SearchProducts(searchName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProducts = Assert.IsAssignableFrom<List<Product>>(okResult.Value);
            Assert.Equal(expectedProducts.Count, returnedProducts.Count);
        }

        [Fact]
        public async Task SearchProducts_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            var searchName = "Product";
            var expectedProducts = new List<Product>();

            var productRepositoryMock = new Mock<IBaseRepository<Product>>();
            productRepositoryMock.Setup(repo => repo.SearchByName(searchName)).ReturnsAsync(expectedProducts);

            var controller = new ProductController(productRepositoryMock.Object);

            // Act
            var result = await controller.SearchProducts(searchName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProducts = Assert.IsAssignableFrom<List<Product>>(okResult.Value);
            Assert.Empty(returnedProducts);
        }


        [Fact]
        public async Task GetProduct_ReturnsOkResultWithProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var expectedProduct = new Product { Id = productId, Name = "Product A", Stock = 10, Price = 20.0m };

            var productRepositoryMock = new Mock<IBaseRepository<Product>>();
            productRepositoryMock.Setup(repo => repo.GetById(productId)).ReturnsAsync(expectedProduct);

            var controller = new ProductController(productRepositoryMock.Object);

            // Act
            var result = await controller.GetProduct(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(expectedProduct.Id, returnedProduct.Id);
        }

        [Fact]
        public async Task GetProduct_ReturnsNotFound()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productRepositoryMock = new Mock<IBaseRepository<Product>>();
            productRepositoryMock.Setup(repo => repo.GetById(productId)).ReturnsAsync((Product?)null);

            var controller = new ProductController(productRepositoryMock.Object);

            // Act
            var result = await controller.GetProduct(productId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Produto não encontrado", notFoundResult.Value);
        }

        [Fact]
        public async Task AddProduct_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(repo => repo.Add(It.IsAny<Product>())).Verifiable();

            var controller = new ProductController(productRepositoryMock.Object);

            // Act
            var result = await controller.AddProduct("Produto A", 10, 20.0m);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AddProduct_NegativePrice_ReturnsBadRequestResult()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();

            var controller = new ProductController(productRepositoryMock.Object);

            // Act
            var result = await controller.AddProduct("Produto B", 15, -10.0m);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("O preço não pode ser negativo.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateProduct_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var existingProduct = new Product { Id = productId, Name = "Produto A", Stock = 10, Price = 20.0m };

            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(repo => repo.GetById(productId)).ReturnsAsync(existingProduct);
            productRepositoryMock.Setup(repo => repo.Update(existingProduct)).Verifiable();

            var controller = new ProductController(productRepositoryMock.Object);

            // Act
            var result = await controller.UpdateProduct(productId, "Produto Atualizado", 15, 30.0m);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateProduct_NegativePrice_ReturnsBadRequestResult()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var existingProduct = new Product { Id = productId, Name = "Produto B", Stock = 15, Price = 25.0m };

            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(repo => repo.GetById(productId)).ReturnsAsync(existingProduct);

            var controller = new ProductController(productRepositoryMock.Object);

            // Act
            var result = await controller.UpdateProduct(productId, "Produto Atualizado", 20, -10.0m);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("O preço não pode ser negativo.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateProduct_ProductNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var productId = Guid.NewGuid();

            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(repo => repo.GetById(productId)).ReturnsAsync((Product)null);

            var controller = new ProductController(productRepositoryMock.Object);

            // Act
            var result = await controller.UpdateProduct(productId, "Produto Atualizado", 20, 30.0m);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Produto não encontrado.", notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteProduct_ProductFound_ReturnsOkResult()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productRepositoryMock = new Mock<IBaseRepository<Product>>();
            productRepositoryMock.Setup(repo => repo.GetById(productId)).ReturnsAsync(new Product { Id = productId });

            var controller = new ProductController(productRepositoryMock.Object);

            // Act
            var result = await controller.DeleteProduct(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal("Produto excluído com sucesso!", okResult.Value);
        }

        [Fact]
        public async Task DeleteProduct_ProductNotFound_ReturnsNotFound()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productRepositoryMock = new Mock<IBaseRepository<Product>>();
            productRepositoryMock.Setup(repo => repo.GetById(productId)).ReturnsAsync((Product)null);

            var controller = new ProductController(productRepositoryMock.Object);

            // Act
            var result = await controller.DeleteProduct(productId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Produto não encontrado", notFoundResult.Value);
        }
    }
}

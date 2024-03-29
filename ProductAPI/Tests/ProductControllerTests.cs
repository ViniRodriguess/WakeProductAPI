/*using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Controllers;
using ProductAPI.Models;
using ProductAPI.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ProductAPI.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public async Task GetAllProducts_ReturnsOkResult()
        {
            // Arrange
            var mockContext = new Mock<DataContext>();
            _ = mockContext.Setup(x => x.Products.ToListAsync()).ReturnsAsync(new List<Product>());

            var controller = new ProductController(mockContext.Object);

            // Act
            var result = await controller.GetAllProducts();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetProduct_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var productId = System.Guid.NewGuid();
            var product = new Product { Id = productId, Name = "Test Product", Stock = 10, Price = 100 };

            var mockSet = new Mock<DbSet<Product>>();
            mockSet.Setup(x => x.FindAsync(productId)).ReturnsAsync(product);

            var mockContext = new Mock<DataContext>();
            mockContext.Setup(x => x.Products).Returns(mockSet.Object);

            var controller = new ProductController(mockContext.Object);

            // Act
            var result = await controller.GetProduct(productId);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        // Add more tests for other methods like AddProduct, UpdateProduct, DeleteProduct
    }
}*/

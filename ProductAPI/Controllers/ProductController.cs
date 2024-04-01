using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using ProductAPI.Repository.Interfaces;
using System.Linq.Expressions;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IBaseRepository<Product> _productRepository;

        public ProductController(IBaseRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            var products = await _productRepository.GetAll();
            return Ok(products);
        }

        [HttpGet("order")]
        public async Task<ActionResult<List<Product>>> GetOrderedProducts(string sortBy)
        {
            Expression<Func<Product, object>> orderBy;

            switch (sortBy.ToLower())
            {
                case "name":
                    orderBy = p => p.Name;
                    break;
                case "stock":
                    orderBy = p => p.Stock;
                    break;
                case "price":
                    orderBy = p => p.Price;
                    break;
                default:
                    return BadRequest("Campo de ordenação inválido.");
            }

            var products = await _productRepository.GetAllOrderedBy(orderBy);
            return Ok(products);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Product>>> SearchProducts(string name)
        {
            var products = await _productRepository.SearchByName(name);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var product = await _productRepository.GetById(id);
            if (product == null)
                return NotFound("Produto não encontrado");

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(string name, int stock, decimal price)
        {
            if (price < 0)
            {
                return BadRequest("O preço não pode ser negativo.");
            }

            var product = new Product { Name = name, Stock = stock, Price = price };

            try
            {
                await _productRepository.Add(product);
                return Ok(product);
            }
            catch
            {
                return StatusCode(500, "Ocorreu um erro ao adicionar o produto.");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, string name, int stock, decimal price)
        {
            if (price < 0)
            {
                return BadRequest("O preço não pode ser negativo.");
            }

            var existingProduct = await _productRepository.GetById(id);
            if (existingProduct == null)
            {
                return NotFound("Produto não encontrado.");
            }

            existingProduct.Name = name;
            existingProduct.Stock = stock;
            existingProduct.Price = price;

            try
            {
                await _productRepository.Update(existingProduct);
                return Ok(existingProduct);
            }
            catch
            {
                return StatusCode(500, "Ocorreu um erro ao atualizar o produto.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(Guid id)
        {
            var existingProduct = await _productRepository.GetById(id);
            if (existingProduct == null)
                return NotFound("Produto não encontrado");

            await _productRepository.Delete(id);
            return Ok("Produto excluído com sucesso!");
        }
    }
}

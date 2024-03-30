using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Models;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _context;

         public ProductController(DataContext context)
         {
             _context = context;
         }
         
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            
            return Ok(products);
        }

        [HttpGet("order")]
        public async Task<ActionResult<List<Product>>> GetOrderedProducts(string sortBy)
        {
            IQueryable<Product> query = _context.Products;

            switch (sortBy.ToLower())
            {
                case "name":
                    query = query.OrderBy(p => p.Name);
                    break;
                case "stock":
                    query = query.OrderBy(p => p.Stock);
                    break;
                case "price":
                    query = query.OrderBy(p => p.Price);
                    break;
                default:
                    return BadRequest("Campo de ordenação inválido.");
            }

            var products = await query.ToListAsync();
            return Ok(products);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Product>>> SearchProducts(string name)
        {
            var products = await _context.Products
                                        .Where(p => p.Name.Contains(name))
                                        .ToListAsync();

            if (products.Count == 0)
                return NotFound("Nenhum produto encontrado com esse nome.");

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null)
                return NotFound("Produto não encontrado");

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<List<Product>>> AddProduct(string name, int stock, decimal price)
        {
            if (price < 0)
            {
                return BadRequest("O preço do produto não pode ser negativo.");
            }

            var newProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = name,
                Stock = stock,
                Price = price
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return Ok("Produto criado com sucesso!");
        }


        /*[HttpPost]
        public async Task<ActionResult<Product>> AddProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            product.Id = Guid.NewGuid();
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }*/

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Product>>> UpdateProduct(Guid id, string name, int stock, decimal price)
        {
            if (price < 0)
            {
                return BadRequest("O preço do produto não pode ser negativo.");
            }

            var dbProduct = await _context.Products.FindAsync(id);
            if (dbProduct is null)
                return NotFound("Produto não encontrado");

            dbProduct.Name = name;
            dbProduct.Stock = stock;
            dbProduct.Price = price;

            await _context.SaveChangesAsync();

            return Ok(dbProduct);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Product>>> DeleteProduct(Guid id)
        {
            var dbProduct = await _context.Products.FindAsync(id);
            if (dbProduct == null)
            {
                return NotFound("Produto não encontrado");
            }

            _context.Products.Remove(dbProduct);
            await _context.SaveChangesAsync();

            return Ok(dbProduct);
        }




    }
}

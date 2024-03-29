using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Models;
using System;

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
         
        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            
            return Ok(products);
        }

        // GET: api/Product
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null)
                return NotFound("Produto não encontrado");

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<List<Product>>> AddProduct(string name, int stock, decimal price)
        {
            var newProduct = new Product
            {
                Name = name,
                Stock = stock,
                Price = price
            };
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return Ok("Produto criado com sucesso!");
        }

        [HttpPut]
        public async Task<ActionResult<List<Product>>> UpdateProduct(int id, string name, int stock, decimal price)
        {
            var dbProduct = await _context.Products.FindAsync(id);
            var res = await _context.Products.ToListAsync();
            if (dbProduct is null)
                return NotFound("Produto não encontrado");

            dbProduct.Name = name;
            dbProduct.Stock = stock;
            dbProduct.Price = price;

            await _context.SaveChangesAsync();

            return Ok(await _context.Products.ToListAsync());
        }

        [HttpDelete]
        public async Task<ActionResult<List<Product>>> DeleteProduct(int id)
        {
            var dbProduct = await _context.Products.FindAsync(id);
            if (dbProduct is null)
                return NotFound("Produto não encontrado");

            _context.Products.Remove(dbProduct);
            await _context.SaveChangesAsync();

            return Ok(await _context.Products.ToListAsync());
        }
    }
}

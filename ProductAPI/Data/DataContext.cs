using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ProductAPI.Models;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;

namespace ProductAPI.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Product> Products { get; set; }



        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
      
        }
    }
}

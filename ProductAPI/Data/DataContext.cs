﻿using Microsoft.EntityFrameworkCore;
using ProductAPI.Models;

namespace ProductAPI.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineStoreApp.Models;

namespace OnlineStoreApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemImage> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShoppingCartItem>().ToTable("tbl_ShoppingCartItem").HasKey(s => s.Id);
            modelBuilder.Entity<ItemImage>().ToTable("tbl_Images").HasKey(i => i.ImageId);
            modelBuilder.Entity<Item>()
                .ToTable("tbl_Items")
                .HasData
                (
                    new Item { ItemId = 1, Name = "CPU", Price = 50 },
                    new Item { ItemId = 2, Name = "Memory", Price = 10 },
                    new Item { ItemId = 3, Name = "Motherboard", Price = 30 },
                    new Item { ItemId = 4, Name = "Graphics card", Price = 100 },
                    new Item { ItemId = 5, Name = "Power supply", Price = 20 }
                );
        }
    }
}
using Microsoft.EntityFrameworkCore;
using webapi.Model;

namespace webapi.Data
{
    public class RestaurantContext : DbContext
    {
        public RestaurantContext(DbContextOptions<RestaurantContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    CustomerId = 1,
                    Name = "Customer one"
                },
                new Customer
                {
                    CustomerId = 2,
                    Name = "Customer two"
                },
                new Customer
                {
                    CustomerId = 3,
                    Name = "Customer three"
                }
            );
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    Description = "Slice of pizza",
                    Price = 2.99m
                },
                new Product
                {
                    ProductId = 2,
                    Description = "Hamburger",
                    Price = 7.99m
                },
                new Product
                {
                    ProductId = 3,
                    Description = "Fries",
                    Price = 3.00m
                },
                new Product
                {
                    ProductId = 4,
                    Description = "Bottle of beer",
                    Price = 2.49m
                }
            );
        }

        public DbSet<webapi.Model.Order> Order { get; set; }
    }
}
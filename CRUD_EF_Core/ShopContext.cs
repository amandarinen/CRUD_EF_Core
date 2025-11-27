using CRUD_EF_Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace CRUD_EF_Core
{
    public class ShopContext : DbContext
    {
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderRow> OrderRows => Set<OrderRow>();
        public DbSet<Product> Products => Set<Product>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(AppContext.BaseDirectory, "shop.db");
            optionsBuilder.UseSqlite($"Filename= {dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(e =>
            {
                e.HasKey(customer => customer.CustomerId);

                e.Property(customer => customer.Name)
                .IsRequired().HasMaxLength(100);

                e.Property(customer => customer.Email).IsRequired().HasMaxLength(100);

                e.Property(customer => customer.City).HasMaxLength(100);

                e.HasIndex(customer => customer.Email).IsUnique();
            });

            modelBuilder.Entity<Order>(e =>
            {
                e.HasKey(order => order.OrderId);

                e.Property(order => order.OrderDate);

                e.Property(order => order.Status).HasMaxLength(50).IsRequired();

                e.HasOne(order => order.Customer)
                .WithMany(customer => customer.Orders)
                .HasForeignKey(order => order.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<OrderRow>(e =>
            {
                e.HasKey(orderrow => orderrow.OrderRowId);

                e.Property(orderrow => orderrow.Quantity);

                e.Property(orderrow => orderrow.UnitPrice);

                e.HasOne(orderrow => orderrow.Order)
                .WithMany(orderrow => orderrow.OrderRows)
                .HasForeignKey(orderrow => orderrow.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(orderrow => orderrow.Product)
                .WithMany()
                .HasForeignKey(orderrow => orderrow.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Product>(e =>
            {
                e.HasKey(product => product.ProductId);

                e.Property(product => product.ProductPrice).IsRequired();

                e.Property(product => product.ProductName).IsRequired().HasMaxLength(100);

                e.Property(product => product.ProductDescription).HasMaxLength(250);
            });
        }
    }
}

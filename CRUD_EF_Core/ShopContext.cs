using CRUD_EF_Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace CRUD_EF_Core
{
    public class ShopContext : DbContext
    {
        //Mappar datan mot tabellerna i databasen
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderRow> OrderRows => Set<OrderRow>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<OrderSummary> OrderSummaries => Set<OrderSummary>();
        public DbSet<CustomerOrderCountView> CustomerOrderCountViews => Set<CustomerOrderCountView>();
        public DbSet<ProductSalesView> ProductSalesViews => Set<ProductSalesView>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(AppContext.BaseDirectory, "shop.db");
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(e =>
            {
                e.HasKey(customer => customer.CustomerId);

                e.Property(customer => customer.Name).IsRequired().HasMaxLength(100);

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
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderRow>(e =>
            {
                e.HasKey(orderrow => orderrow.OrderRowId);

                e.Property(orderrow => orderrow.Quantity);

                e.Property(orderrow => orderrow.UnitPrice);

                e.HasOne(orderrow => orderrow.Order)
                .WithMany(orderrow => orderrow.OrderRows)
                .HasForeignKey(orderrow => orderrow.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

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

                e.HasOne(product => product.Category)
                .WithMany(category => category.Products)
                .HasForeignKey(product => product.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Category>(e =>
            {
                e.HasKey(category => category.CategoryId);

                e.Property(category => category.CategoryName).IsRequired().HasMaxLength(100);

                e.Property(category => category.CategoryDescription).HasMaxLength(250);

                e.HasIndex(category => category.CategoryName).IsUnique();
            });

            modelBuilder.Entity<OrderSummary>(e =>
            {
                e.HasNoKey(); // saknar PK

                e.ToView("OrderSummary"); //kopplar tabellen mot SQLite, behöver inte ha matchande modellnamn bara samma som våran table
            });

            modelBuilder.Entity<CustomerOrderCountView>(e =>
            {
                e.HasNoKey();

                e.ToView("CustomerOrderCountView");
            });

            modelBuilder.Entity<ProductSalesView>(e =>
            {
                e.HasNoKey();

                e.ToView("ProductSalesView");
            });
        }
    }
}

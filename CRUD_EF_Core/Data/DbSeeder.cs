using CRUD_EF_Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_EF_Core.Data
{
    public static class DbSeeder
    {

        public static async Task SeedAsync()

        {
            using var db = new ShopContext();

            await db.Database.MigrateAsync();

            if (!await db.Customers.AnyAsync())
            {
                db.Customers.AddRange(
                    new Customer { Name = "Anna Andersson", Email = "anna.andersson@gmail.com", City = "Stockholm" },
                    new Customer { Name = "Peter Persson", Email = "peter@persson.com", City = "Malmö" },
                    new Customer { Name = "Olof Olson", Email = "olof.olson@live.se", City = "Umeå" },
                    new Customer { Name = "Gunilla Gran", Email = "gunillagran@outlook.com", City = "Göteborg" }
                    );
                await db.SaveChangesAsync();
                Console.WriteLine("Seeded db with customers!");
            }

            if (!await db.Categories.AnyAsync())
            {
                db.Categories.AddRange(
                    new Category { CategoryName = "Electronics", CategoryDescription = "Devices and gadgets" },
                    new Category { CategoryName = "Accessories", CategoryDescription = "Phone and computer accessories" },
                    new Category { CategoryName = "Wearables", CategoryDescription = "Smart watches and fitness trackers" },
                    new Category { CategoryName = "Gaming", CategoryDescription = "Gaming peripherals and consoles" }
                );
                await db.SaveChangesAsync();
                Console.WriteLine("Seeded db with categories!");
            }

            if (!await db.Products.AnyAsync())
            {
                db.Products.AddRange(
                    new Product { ProductName = "Headphones", ProductDescription = "Wireless headphones", ProductPrice = 799 , CategoryId = 2},
                    new Product { ProductName = "Keyboard", ProductDescription = "backlit mechanical keyboard", ProductPrice = 599 , CategoryId = 1 },
                    new Product { ProductName = "Monitor", ProductDescription = "4K Display", ProductPrice = 349 , CategoryId = 2 },
                    new Product { ProductName = "Smart Watch", ProductDescription = "Smart fitness watch", ProductPrice = 999 , CategoryId = 3 },
                    new Product { ProductName = "Powerbank", ProductDescription = "Fast charging powerbank.", ProductPrice = 200 , CategoryId = 2 }
                    );
                await db.SaveChangesAsync();
                Console.WriteLine("Seeded db with products!");
            }

            if (!await db.Orders.AnyAsync())
            {
                db.Orders.AddRange(
                    new Order { CustomerId = 1, OrderDate = DateTime.Now.AddDays(-10), Status = Status.Completed },
                    new Order { CustomerId = 2, OrderDate = DateTime.Now.AddDays(-2), Status = Status.Processing },
                    new Order { CustomerId = 1, OrderDate = DateTime.Now.AddDays(-5), Status = Status.Completed },
                    new Order { CustomerId = 3, OrderDate = DateTime.Now.AddDays(-4), Status = Status.Canceled }
                    );
                await db.SaveChangesAsync();
                Console.WriteLine("Seeded db with orders!");
            }

            if (!await db.OrderRows.AnyAsync())
            {
                db.OrderRows.AddRange(
                    new OrderRow { OrderId = 1, ProductId = 1, Quantity = 4, UnitPrice = 799 },
                    new OrderRow { OrderId = 2, ProductId = 2, Quantity = 2, UnitPrice = 599 },
                    new OrderRow { OrderId = 3, ProductId = 3, Quantity = 6, UnitPrice = 349 }
                    );
                await db.SaveChangesAsync();
                Console.WriteLine("Seeded db with order rows!");
            }
        }
    }
}

using CRUD_EF_Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CRUD_EF_Core.Services
{
    public class ProductServices
    {
        public static async Task ListProductAsync()
        {
            using var db = new ShopContext();

            var rows = await db.Products.AsNoTracking().OrderBy(product => product.ProductId)
                                        .Include(c => c.Category).ToListAsync();
            Console.WriteLine("ID | Name | Description | Price | Category");
            foreach (var row in rows)
            {
                Console.WriteLine($"{row.ProductId} | {row.ProductName} | {row.ProductDescription} | {row.ProductPrice} | {row.Category?.CategoryName}");
            }
        }

        public static async Task DeleteProductAsync(int id)
        {
            using var db = new ShopContext();
            var product = await db.Products.FirstOrDefaultAsync(product => product.ProductId == id);
            if (product == null)
            {
                Console.WriteLine("Product not found!");
                return;
            }
            db.Products.Remove(product);
            try
            {
                await db.SaveChangesAsync();
                Console.WriteLine("Product deleted!");
            }
            catch (DbUpdateException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public static async Task EditProductAsync(int id)
        {
            using var db = new ShopContext();

            //hämta raden vi vill uppdatera
            var product = await db.Products.FirstOrDefaultAsync(product => product.ProductId == id);
            if (product == null)
            {
                Console.WriteLine("Product not found");
                return;
            }

            //visar nuvarande värden: uppdatera name för en specifik product
            Console.WriteLine($"{product.ProductName} ");
            var name = Console.ReadLine()?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(name))
            {
                product.ProductName = name;
            }

            //uppdatera description för en specifik product
            Console.Write($"{product.ProductDescription}");
            var description = Console.ReadLine()?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(description))
            {
                product.ProductDescription = description;
            }

            //uppdatera priset på en product
            Console.WriteLine($"{product.ProductPrice}");
            var priceInput = Console.ReadLine()?.Trim() ?? string.Empty;
            if (decimal.TryParse(priceInput, out var price) && price >= 0)
            {
                product.ProductPrice = price;
            }

            try
            {
                await db.SaveChangesAsync();
                Console.WriteLine("Edited!");
            }
            catch (DbUpdateException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public static async Task AddProductAsync()
        {
            Console.WriteLine("All Categories: ");
            await CategoryServices.ListCategoryAsync();

            Console.WriteLine("Choose Category Id");
            var CatId = Console.ReadLine();

            if (!int.TryParse(CatId, out var categoryId))
            {
                Console.WriteLine("Invalid category ID.");
                return;
            }

            Console.WriteLine("Name: ");
            var name = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(name) || name.Length > 100)
            {
                Console.WriteLine("Name is required (max 100).");
                return;
            }

            Console.WriteLine("Description (optional): ");
            var desc = Console.ReadLine();

            Console.WriteLine("Price: ");
            var priceInput = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(priceInput) || !decimal.TryParse(priceInput, out var price) || price <= 0)
            {
                Console.WriteLine("Price is required and must be more than 0.");
                return;
            }

            using var db = new ShopContext();
            db.Products.Add(new Product { ProductName = name, ProductDescription = desc, ProductPrice = price, CategoryId = categoryId });
            try
            {
                await db.SaveChangesAsync();
                Console.WriteLine("Product added!");
            }
            catch (DbUpdateException exception)
            {
                Console.WriteLine("DB Error!" + exception.GetBaseException().Message);
            }
        }

        public static async Task ProductSalesAsync()

        {
            using var db = new ShopContext();
            var products = await db.ProductSalesViews
                .OrderByDescending(p => p.ProductId)
                .ToListAsync();

            foreach(var product in products)
            {
                Console.WriteLine($"ProductId: {product.ProductId} | Total Quantity Sold: {product.TotalQuantitySold}");
            }
        }
    }
}
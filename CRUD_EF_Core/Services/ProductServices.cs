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
    /// <summary>
    /// Provides a collection of asynchronous methods for managing products.
    /// </summary>
    /// <remarks>This class includes methods for listing, adding, editing, deleting, and querying products, 
    /// as well as retrieving product sales data. </remarks>
    public class ProductServices
    {
        /// <summary>
        /// Retrieves and displays a paginated list of products, including their details and associated categories.
        /// </summary>
        /// <remarks>This method shows a list of products and includes their associated categories. 
        /// The results are displayed in the console in a tabular format, along with pagination details.</remarks>
        /// <param name="page">The page number to retrieve. Must be greater than or equal to 1.</param>
        /// <param name="pageSize">The number of products to include per page. Must be greater than 0.</param>
        /// <returns></returns>
        public static async Task ListProductAsync(int page, int pageSize)
        {
            using var db = new ShopContext();

            var sw = System.Diagnostics.Stopwatch.StartNew();

            var prod = db.Products.AsNoTracking()
                .OrderBy(product => product.ProductId)
                .Include(c => c.Category);

            sw.Stop();
            Console.WriteLine($"Total time for query {sw.ElapsedMilliseconds} ms");

            var products = await prod
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await prod.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            Console.WriteLine($"Page = {page}/{totalPages}, pageSize = {pageSize}");

            Console.WriteLine("Product ID | Name | Description | Price | Category");

            foreach (var product in products)
            {
                Console.WriteLine($"{product.ProductId} | {product.ProductName} | {product.ProductDescription} | {product.ProductPrice} | {product.Category?.CategoryName}");
            }
        }

        /// <summary>
        /// Deletes a product with the specified identifier.
        /// </summary>
        /// <remarks>If no product with the specified identifier exists, the method logs a message and no
        /// action is taken and if deletion fails the exception message is logged.</remarks>
        /// <param name="id">The unique identifier of the product to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
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

        /// <summary>
        /// Updates the details of an existing product in the database.
        /// </summary>
        /// <remarks>This method updates the products name, description and price. 
        /// If the product is not found, a message is displayed without making changes.</remarks>
        /// <param name="id">The unique identifier of the product to be updated.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task EditProductAsync(int id)
        {
            using var db = new ShopContext();
            var product = await db.Products.FirstOrDefaultAsync(product => product.ProductId == id);

            if (product == null)
            {
                Console.WriteLine("Product not found");
                return;
            }

            Console.WriteLine($"{product.ProductName} ");
            var name = Console.ReadLine()?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(name))
            {
                product.ProductName = name;
            }

            Console.Write($"{product.ProductDescription}");
            var description = Console.ReadLine()?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(description))
            {
                product.ProductDescription = description;
            }

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

        /// <summary>
        /// Adds a new product.
        /// </summary>
        /// <remarks> The method retrives and displays all available catagories to help the user select a valid category ID.
        /// This method adds a product including its name, description, price, and associated category. 
        /// It validates to ensure the input is correct and saves the product to the database. 
        /// The product name is required and must not exceed 100 characters. The price must be a positive decimal value.</remarks>
        /// <returns></returns>
        public static async Task AddProductAsync()
        {
            Console.WriteLine("All Categories: ");
            using var db = new ShopContext();
            var categories = await db.Categories
                .AsNoTracking()
                .OrderBy(category => category.CategoryId)
                .ToListAsync();

            Console.WriteLine("Category ID | Name | Description");

            foreach (var category in categories)
            {
                Console.WriteLine($"{category.CategoryId} | {category.CategoryName} | {category.CategoryDescription}");
            }

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

            db.Products.Add(new Product 
            { 
                ProductName = name, 
                ProductDescription = desc, 
                ProductPrice = price, 
                CategoryId = categoryId 
            });

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

        /// <summary>
        /// Asynchronously retrieves and displays product sales data, ordered by product ID in descending order.
        /// </summary>
        /// <remarks>This method shows the product sales information.
        /// Each product's ID and total quantity sold are displayed. </remarks>
        /// <returns>A task that represents the asynchronous operation.</returns>
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

        /// <summary>
        /// Shows a list of products for the specified category.
        /// </summary>
        /// <remarks>This method queries the database for products that belong to the specified category, 
        /// orders them by price, and includes their associated category information. </remarks>
        /// <param name="categoryId">The identifier of the category whose products should be listed. Must be a valid category ID.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task ListProductsByCategoryAsync(int categoryId)
        {
            using var db = new ShopContext();
            var products = await db.Products
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.Category)
                .OrderBy(p => (int)p.ProductPrice)
                .ToListAsync();

            Console.WriteLine("Product ID | Name | Price | Description | Category");

            foreach (var product in products)
            {
                Console.WriteLine($"{product.ProductId} | {product.ProductName} | {product.ProductDescription} | {product.ProductPrice} | {product.Category?.CategoryName}");
            }
        }
    }
}
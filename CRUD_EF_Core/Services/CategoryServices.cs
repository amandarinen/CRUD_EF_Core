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
    /// Provides services for managing categories including listing, adding, editing, and deleting categories.
    /// </summary>
    public class CategoryServices
    {
       /// <summary>
       /// Retrieves and displays a paginated list of categories from the database.
       /// </summary>
       /// <remarks>This method shows a list of all categories, orders them by their unique
       /// identifier, and retrieves the specified page of results. The total number of pages is calculated based on the
       /// total count of categories and the specified page size.</remarks>
       /// <param name="page">The page number to retrieve. Must be greater than or equal to 1.</param>
       /// <param name="pageSize">The number of categories to include per page. Must be greater than 0.</param>
       /// <returns></returns>
        public static async Task ListCategoryAsync(int page, int pageSize)
        {
            using var db = new ShopContext();
            var cat = db.Categories
                .AsNoTracking()
                .OrderBy(category => category.CategoryId);

            var categories = await cat
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await cat.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            Console.WriteLine($"Page = {page}/{totalPages}, pageSize = {pageSize}");

            Console.WriteLine("Category ID | Name | Description");

            foreach (var category in categories)
            {
                Console.WriteLine($"{category.CategoryId} | {category.CategoryName} | {category.CategoryDescription}");
            }
        }

        /// <summary>
        /// Updates the name and description of a category in the database based on the specified category ID.
        /// </summary>
        /// <remarks> This method updates a category from the database based on the users input.
        /// If no input is provided, the existing values remain unchanged. The changes are then saved to
        /// the database.</remarks>
        /// <param name="id">The unique identifier of the category to be updated.</param>
        /// <returns></returns>
        public static async Task EditCategoryAsync(int id)
        {
            using var db = new ShopContext();
            var category = await db.Categories.FirstOrDefaultAsync(category => category.CategoryId == id);

            if (category == null)
            {
                Console.WriteLine("Category not found");
                return;
            }

            Console.WriteLine($"{category.CategoryName} ");
            var name = Console.ReadLine()?.Trim() ?? string.Empty;

            if (!string.IsNullOrEmpty(name))
            {
                category.CategoryName = name;
            }

            Console.Write($"{category.CategoryDescription}");
            var description = Console.ReadLine()?.Trim() ?? string.Empty;

            if (!string.IsNullOrEmpty(description))
            {
                category.CategoryDescription = description;
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
        /// Deletes a category with the specified identifier from the database.
        /// </summary>
        /// <remarks> If the category with the specified identifier does not exist, the method logs a
        /// message and exits without making any changes. If an error occurs while saving changes to the database, the
        /// exception message is logged.</remarks>
        /// <param name="id">The unique identifier of the category to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task DeleteCategoryAsync(int id)
        {
            using var db = new ShopContext();
            var category = await db.Categories.FirstOrDefaultAsync(category => category.CategoryId == id);

            if (category == null)
            {
                Console.WriteLine("Category not found!");
                return;
            }
            db.Categories.Remove(category);

            try
            {
                await db.SaveChangesAsync();
                Console.WriteLine("Category deleted!");
            }

            catch (DbUpdateException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        /// <summary>
        /// Adds a new category.
        /// </summary>
        /// <remarks>Adds a new category based on the users input of name and an optional description. 
        /// The category name is required and must not exceed 100 characters. If the name is invalid,  the operation is
        /// aborted. The method saves the new category to the database and handles potential database update
        /// exceptions, such as duplicate category names.</remarks>
        /// <returns></returns>
        public static async Task AddCategoryAsync()
        {
            Console.WriteLine("Name of new category: ");
            var name = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(name) || name.Length > 100)
            {
                Console.WriteLine("Name is required (max 100).");
                return;
            }

            Console.WriteLine("Description (optional): ");
            var desc = Console.ReadLine()?.Trim() ?? string.Empty;

            using var db = new ShopContext();
            db.Categories.Add(new Category 
            { 
                CategoryName = name, 
                CategoryDescription = desc 
            });

            try
            {
                await db.SaveChangesAsync();
                Console.WriteLine("Category added!");
            }

            catch (DbUpdateException exception)
            {
                Console.WriteLine("DB Error (maybe duplicates?)!" + exception.GetBaseException().Message);
            }
        }
    }
}

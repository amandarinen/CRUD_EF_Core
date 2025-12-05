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
    public class CategoryServices
    {
        public static async Task ListCategoryAsync()
        {
            using var db = new ShopContext();

            //AsNoTracking = snabbare för read-only scenarion. (ingen change tracking)
            var rows = await db.Categories.AsNoTracking().OrderBy(category => category.CategoryId).ToListAsync();
            Console.WriteLine("ID | Name | Description");
            foreach (var row in rows)
            {
                Console.WriteLine($"{row.CategoryId} | {row.CategoryName} | {row.CategoryDescription}");
            }
        }

        public static async Task EditCategoryAsync(int id)
        {
            using var db = new ShopContext();

            //hämta raden vi vill uppdatera
            var category = await db.Categories.FirstOrDefaultAsync(category => category.CategoryId == id);
            if (category == null)
            {
                Console.WriteLine("Category not found");
                return;
            }

            //visar nuvarande värden: uppdatera name för en specifik category
            Console.WriteLine($"{category.CategoryName} ");
            var name = Console.ReadLine()?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(name))
            {
                category.CategoryName = name;
            }

            //uppdatera description för en specifik category
            Console.Write($"{category.CategoryDescription}");
            var description = Console.ReadLine()?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(description))
            {
                category.CategoryDescription = description;
            }

            //Uppdatera DBn med våra ändringar
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

        public static async Task AddCategoryAsync()
        {
            Console.WriteLine("Name: ");
            var name = Console.ReadLine()?.Trim() ?? string.Empty;

            //enkel validering
            if (string.IsNullOrEmpty(name) || name.Length > 100)
            {
                Console.WriteLine("Name is required (max 100).");
                return;
            }
            Console.WriteLine("Description (optional): ");
            var desc = Console.ReadLine()?.Trim() ?? string.Empty;

            using var db = new ShopContext();
            db.Categories.Add(new Category { CategoryName = name, CategoryDescription = desc });
            try
            {
                //spara våra ändringar; trigga en INSERT + all validering/constrains
                await db.SaveChangesAsync();
                Console.WriteLine("Category added!");
            }
            catch (DbUpdateException exception)
            {
                //hit kommer vi tex om Unique-indexet på CategoryName bryts
                Console.WriteLine("DB Error (maybe duplicates?)!" + exception.GetBaseException().Message);
            }
        }
    }
}

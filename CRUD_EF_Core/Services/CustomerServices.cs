using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using CRUD_EF_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace CRUD_EF_Core.Services
{
    public class CustomerServices
    {
        public static async Task ListCustomerAsync()
        {
            var db = new ShopContext();
            var rows = await db.Customers.AsNoTracking().OrderBy(customer => customer.CustomerId).ToListAsync();
            Console.WriteLine("Customer Id | Customer Name | Email | City ");
            foreach (var row in rows)
            {
                Console.WriteLine($"{row.CustomerId} | {row.Name} | {row.Email} | {row.City}");
            }
        }

        public static async Task AddCustomerAsync()
        {
            Console.WriteLine("Customer Name: ");
            var name = Console.ReadLine()?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(name) || name.Length > 100)
            {
                Console.WriteLine("Name is required (max 100 characters).");
                return;
            }

            Console.WriteLine("Email: ");
            var email = Console.ReadLine()?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(email) || email.Length > 100)
            {
                Console.WriteLine("Email is required (max 100 characters).");
                return;
            }

            Console.WriteLine("City: ");
            var city = Console.ReadLine()?.Trim() ?? string.Empty;
            if (city.Length > 100)
            {
                Console.WriteLine("Name of City (max 100 characters).");
                return;
            }

            using var db = new ShopContext();
            db.Customers.Add(new Customer { Name = name, Email = email, City = city});
            try
            {
                await db.SaveChangesAsync();
                Console.WriteLine("Customer added!");
            }
            catch (DbUpdateException exception)
            {
                Console.WriteLine("DB Error!" + exception.GetBaseException().Message);
            }
        }

        public static async Task EditCustomerAsync(int id)
        {
            using var db = new ShopContext();

            var customer = await db.Customers.FirstOrDefaultAsync(customer => customer.CustomerId == id);
            if (customer == null)
            {
                Console.WriteLine("Customer not found");
                return;
            }

            Console.WriteLine($"Edit name ({customer.Name}): ");
            var name = Console.ReadLine()?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(name))
            {
                customer.Name = name;
            }

            Console.WriteLine($"Edit email ({customer.Email}):  ");
            var email = Console.ReadLine()?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(email))
            {
                customer.Email = email;
            }

            Console.WriteLine($"Edit city ({customer.City}): ");
            var city = Console.ReadLine()?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(city))
            {
                customer.City = city;
            }

            try
            {
                await db.SaveChangesAsync();
                Console.WriteLine("Customer edited!");
            }
            catch (DbUpdateException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}

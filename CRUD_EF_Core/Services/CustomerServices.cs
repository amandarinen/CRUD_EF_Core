using CRUD_EF_Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace CRUD_EF_Core.Services
{
    /// <summary>
    /// Provides a collection of asynchronous methods for managing customer data, including listing,  adding, editing,
    /// and deleting customers, as well as retrieving customer order statistics.
    /// </summary>
    public class CustomerServices
    {
        /// <summary>
        /// Retrieves and displays a paginated list of customers.
        /// </summary>
        /// <remarks>This method queries the database for orders and displays the results in a formatted table.  
        /// Pagination is applied based on the specified page and page size.
        /// <param name="page">The page number to retrieve. Must be greater than or equal to 1.</param>
        /// <param name="pageSize">The number of orders to include per page. Must be greater than 0.</param>
        /// <returns></returns>
        public static async Task ListCustomerAsync(int page, int pageSize)
        {
            var db = new ShopContext();
            var query = db.Customers
                .AsNoTracking()
                .OrderBy(customer => customer.CustomerId);

            var customers = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            Console.WriteLine($"Page = {page}/{totalPages}, pageSize = {pageSize}");

            Console.WriteLine("Customer Id | Customer Name | Email | City ");

            foreach (var customer in customers)
            {
                Console.WriteLine($"{customer.CustomerId} | {customer.Name} | {customer.Email} | {customer.City}");
            }
        }

        /// <summary>
        /// Adds a new customer to the database including a securely hashed personnummer.
        /// </summary>
        /// <remarks>This method prompts the user to input name, email, personnummer and city via the console.</remarks>
        /// <returns></returns>
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

            Console.WriteLine("Personnummer (YYYYMMDDXXXX): ");
            var personnummer = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(personnummer) || personnummer.Length != 12)
            {
                Console.WriteLine("Personnummer (max 12 characters).");
                return;
            }

            var salt = HashingHelper.GenerateSalt();
            var hash = HashingHelper.HashWithSalt(personnummer, salt);

            using var db = new ShopContext();
            db.Customers.Add(new Customer 
            { 
                Name = name, 
                Email = email, 
                City = city,
                CustomerPersonnummerSalt = salt,
                CustomerPersonnummerHash = hash
            });

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

        /// <summary>
        /// Updates the details of an existing customer in the database based on the provided customer ID.
        /// </summary>
        /// <remarks>This method prompts the user to edit the customer's name, email, and city. If the
        /// customer with the specified ID  does not exist, a message is displayed, and the method exits without making
        /// any changes. Any database update errors are caught and logged to the console.</remarks>
        /// <param name="id">The unique identifier of the customer to be edited. Must correspond to an existing customer in the database.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Deletes a customer with the specified identifier.
        /// </summary>
        /// <remarks>If no customer with the specified identifier exists, the method will log a message
        /// and return without making any changes. </remarks>
        /// <param name="id">The unique identifier of the customer to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task DeleteCustomerAsync(int id)
        {
            using var db = new ShopContext();
            var customer = await db.Customers.FirstOrDefaultAsync(customer => customer.CustomerId == id);

            if (customer == null)
            {
                Console.WriteLine("Customer not found!");
                return;
            }
            db.Customers.Remove(customer);

            try
            {
                await db.SaveChangesAsync();
                Console.WriteLine("Customer deleted!");
            }
            catch (DbUpdateException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        /// <summary>
        /// Retrieves and displays the number of orders for each customer.
        /// </summary>
        /// <remarks>This method uses ShopContext to access the data 
        /// and then uses CustomerOrderCountViews in Models with the necessary data.</remarks>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task NumberOfOrdersAsync()
        {
            using var db = new ShopContext();
            var orders = await db.CustomerOrderCountViews
                .OrderByDescending(c => c.CustomerId)
                .ToListAsync();

            foreach (var order in orders)
            {
                Console.WriteLine($"CustomerId: {order.CustomerId} | NumberOfOrders: {order.NumberOfOrders}");
            }
        }
    }
}

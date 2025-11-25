using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
    }
}

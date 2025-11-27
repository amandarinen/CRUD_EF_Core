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
    public class OrderServices
    {
        public static async Task ListOrderAsync(int page, int pageSize)
        {
            var db = new ShopContext();
            var query = db.Orders
                .AsNoTracking()
                .Include(customer => customer.Customer)
                .Include(orderrow => orderrow.OrderRows)
                .OrderBy(order => order.OrderId);
            Console.WriteLine("Order Id | Order Date | Status | Customer Name | Total Amount ");

            var rows = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            Console.WriteLine($"Page {page}/{totalPages}, pageSize={pageSize}");

            foreach (var row in rows)
            {
                Console.WriteLine($"{row.OrderId} | {row.OrderDate} | {row.Status} | {row.Customer?.Name} | {row.TotalAmount}");
            }
        }

        public static async Task OrderDetailsAsync(int orderId)
        {
            using var db = new ShopContext();

            var order = await db.Orders
                .AsNoTracking()
                .Include(order => order.OrderRows)
                .ThenInclude(order => order.Product)
                .FirstOrDefaultAsync(order => order.OrderId == orderId);

            Console.WriteLine("Product Name | Quantity | Price per unit | OrderRowAmount");
            foreach (var row in order.OrderRows)
            {
                Console.WriteLine($"{row.Product?.ProductName} | {row.Quantity} | {row.UnitPrice} | {row.Quantity * row.UnitPrice}");
            }
            Console.WriteLine($"Total Amount: {order.TotalAmount}");
        }

        public static async Task AddOrderAsync()
        {
            await CustomerServices.ListCustomerAsync();
            using var db = new ShopContext();

            Console.WriteLine("Enter Customer Id for this order: ");
            if (!int.TryParse(Console.ReadLine(), out var customerId))
            {
                Console.WriteLine("Invalid, CustomerId required");
                return;
            }

            var customer = await db.Customers.FindAsync(customerId);
            if (customer == null)
            {
                Console.WriteLine("Customer not found");
                return;
            }

            var newOrder = new Order
            {
                CustomerId = customerId,
                OrderDate = DateTime.Now,
                Status = Status.Processing
            };

            while (true)
            {

                Console.WriteLine("Available products:");
                var products = await db.Products
                    .AsNoTracking()
                    .OrderBy(product => product.ProductId)
                    .ToListAsync();

                Console.WriteLine("Product Id | Name | Price | Description");
                foreach (var product in products)
                {
                    Console.WriteLine($"{product.ProductId} | {product.ProductName} | {product.ProductPrice} | {product.ProductDescription}");
                }

                Console.Write("Enter Product Id: ");
                if (!int.TryParse(Console.ReadLine(), out var productId))
                {
                    Console.WriteLine("Invalid, ProductId required");
                    continue;
                }

                var prod = await db.Products.FindAsync(productId);
                if (prod == null)
                {
                    Console.WriteLine("Product not found");
                    continue;
                }

                Console.Write("Enter Quantity: ");
                if (!int.TryParse(Console.ReadLine(), out var quantity) || quantity <= 0)
                {
                    Console.WriteLine("Invalid quantity");
                    continue;
                }

                var orderRow = new OrderRow
                {
                    ProductId = prod.ProductId,
                    Quantity = quantity,
                    UnitPrice = prod.ProductPrice
                };

                newOrder.OrderRows.Add(orderRow);

                Console.WriteLine("\nDone with order? 'yes' or 'no'.");
                var line = Console.ReadLine()?.Trim() ?? string.Empty;

                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                if (line.Equals("yes", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                if (line.Equals("no", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
            }

            db.Orders.Add(newOrder);
            await db.SaveChangesAsync();

            Console.WriteLine("Order created.");
        }

        public static async Task SortOrderCustomerAsync(int customerId)
        {
            var db = new ShopContext();
            var rows = await db.Orders
                .AsNoTracking()
                .Include(customer => customer.Customer)
                .Include(orderrow => orderrow.OrderRows)
                .Where(customer => customer.CustomerId == customerId)
                .OrderBy(order => order.OrderId)
                .ToListAsync();
            Console.WriteLine("Order Id | Order Date | Status | Customer Name | Total Amount ");
            foreach (var row in rows)
            {
                Console.WriteLine($"{row.OrderId} | {row.OrderDate} | {row.Status} | {row.Customer?.Name} | {row.TotalAmount}");
            }
        }

        public static async Task SortOrderStatusAsync(Status status)
        {
            var db = new ShopContext();
            var rows = await db.Orders
                .AsNoTracking()
                .Include(customer => customer.Customer)
                .Include(orderrow => orderrow.OrderRows)
                .Where(s => s.Status == status)
                .OrderBy(order => order.OrderId)
                .ToListAsync();
            Console.WriteLine("Order Id | Order Date | Status | Customer Name | Total Amount ");
            foreach (var row in rows)
            {
                Console.WriteLine($"{row.OrderId} | {row.OrderDate} | {row.Status} | {row.Customer?.Name} | {row.TotalAmount}");
            }
        }
    }
}
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
    /// Service responsible for managing Orders
    /// </summary>
    public class OrderServices
    {
        /// <summary>
        /// Retrieves and displays a paginated list of orders, including their details, to the console.
        /// </summary>
        /// <remarks>This method queries the database for orders, including associated customer and order
        /// row details,  and displays the results in a formatted table. The output includes the order ID, order date,
        /// status,  customer name, and total amount for each order. 
        /// Pagination is applied based on the specified page and page size
        /// <param name="page">The page number to retrieve. Must be greater than or equal to 1.</param>
        /// <param name="pageSize">The number of orders to include per page. Must be greater than 0.</param>
        /// <returns></returns>
        public static async Task ListOrderAsync(int page, int pageSize)
        {
            var db = new ShopContext();
            var query = db.Orders
                .AsNoTracking()
                .Include(customer => customer.Customer)
                .Include(orderrow => orderrow.OrderRows)
                .OrderBy(order => order.OrderId);

            var rows = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            Console.WriteLine($"Page = {page}/{totalPages}, pageSize = {pageSize}");

            Console.WriteLine("Order Id | Order Date | Status | Customer Name | Total Amount ");

            foreach (var row in rows)
            {
                var totalAmount = row.OrderRows.Sum(or => or.UnitPrice * or.Quantity);

                Console.WriteLine($"{row.OrderId} | {row.OrderDate} | {row.Status} | {row.Customer?.Name} | {totalAmount}");
            }
        }

        /// <summary>
        /// Retrieves and displays the details of a specific order, including its products, quantities, prices, and
        /// total amount.
        /// </summary>
        /// <remarks>This method queries the database for the specified order and its associated order
        /// rows, including product details. If the order is not found, a message indicating this is displayed. The
        /// method outputs the order details to the console, including the product name, quantity, price per unit, and
        /// the total amount for each order row, as well as the overall total amount.</remarks>
        /// <param name="orderId">The unique identifier of the order to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task OrderDetailsAsync(int orderId)
        {
            using var db = new ShopContext();

            var order = await db.Orders
                .AsNoTracking()
                .Include(order => order.OrderRows)
                .ThenInclude(order => order.Product)
                .FirstOrDefaultAsync(order => order.OrderId == orderId);

            if (order == null)
            {
                Console.WriteLine("Order not found.");
                return;
            }

            Console.WriteLine("Product Name | Quantity | Price per unit | OrderRowAmount");

            foreach (var row in order.OrderRows)
            {
                Console.WriteLine($"{row.Product?.ProductName} | {row.Quantity} | {row.UnitPrice} | {row.Quantity * row.UnitPrice}");
            }

            var totalAmount = order.OrderRows.Sum(or => or.UnitPrice * or.Quantity);

            Console.WriteLine($"Total Amount: {totalAmount}");
        }

        /// <summary>
        /// Creates a new order for a customer by interacting with the database and user input.
        /// </summary>
        /// <remarks>This method shows a list of customers so the user can create an order by selecting a customer ID and adding products with
        /// specified quantities.  The order is then saved to the database. The method performs input validation and
        /// ensures that the customer and products exist before proceeding.</remarks>
        /// <returns></returns>
        public static async Task AddOrderAsync()
        {
            using var db = new ShopContext();
            var custs = await db.Customers
                .AsNoTracking()
                .OrderBy(c => c.CustomerId)
                .ToListAsync();

            Console.WriteLine("Customer Id | Customer Name | Email | City ");

            foreach (var cust in custs)
            {
                Console.WriteLine($"{cust.CustomerId} | {cust.Name} | {cust.Email} | {cust.City}");
            }

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
                Status = Status.Processing,
                TotalAmount = 0
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

                Console.WriteLine("\nDone with order? \n > 'yes' or 'no'.");
                var line = Console.ReadLine()?.Trim().ToLowerInvariant() ?? string.Empty;

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

        /// <summary>
        /// Retrieves and displays a sorted list of orders for a specified customer.
        /// </summary>
        /// <remarks>This method shows all orders associated with the specified customer, including related customer and order row details. 
        /// </remarks>
        /// <param name="customerId">The unique identifier of the customer whose orders are to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
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

        /// <summary>
        /// Retrieves and displays a list of orders filtered by their status.
        /// </summary>
        /// <remarks>This method sorts orders with the specified status, including
        /// related customer and order row data. </remarks>
        /// <param name="status">The status of the orders to filter by. Only orders matching this status will be retrieved.</param>
        /// <returns></returns>
        public static async Task SortOrderStatusAsync(Status status)
        {
            var db = new ShopContext();
            var orders = await db.Orders
                .AsNoTracking()
                .Include(customer => customer.Customer)
                .Include(orderrow => orderrow.OrderRows)
                .Where(s => s.Status == status)
                .OrderBy(order => order.OrderId)
                .ToListAsync();

            Console.WriteLine("Order Id | Order Date | Status | Customer Name | Total Amount ");

            foreach (var order in orders)
            {
                Console.WriteLine($"{order.OrderId} | {order.OrderDate} | {order.Status} | {order.Customer?.Name} | {order.TotalAmount}");
            }
        }

        /// <summary>
        /// Retrieves and displays a list of order summaries, sorted by order date.
        /// </summary>
        /// <remarks>This method queries the database for order summaries, including details such as order
        /// ID,  order date, total amount, customer name, and customer email.</remarks>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task ListOrderSummary()
        {
            using var db = new ShopContext();

            var summaries = await db.OrderSummaries
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            Console.WriteLine("Order Id | Order Date | Total Amount | Customer Name | Customer Email");

            foreach(var summary in summaries)
            {
                Console.WriteLine($"{summary.OrderId} | {summary.OrderDate} | {summary.TotalAmount} | {summary.CustomerName} | {summary.CustomerEmail}");
            }
        }
    }
}
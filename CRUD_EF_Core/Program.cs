using CRUD_EF_Core;
using CRUD_EF_Core.Data;
using CRUD_EF_Core.Models;
using CRUD_EF_Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


Console.WriteLine("DB: " + Path.Combine(AppContext.BaseDirectory, "shop.db"));

await DbSeeder.SeedAsync();   

while (true)
{
     Console.WriteLine("\nMain Menu: \n| 1.customers | \n| 2.products | \n| 3.categories | \n| 4.orders | \n| 5.sortorders | \n| exit");
     Console.WriteLine("> ");
     var line = Console.ReadLine()?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(line))
        {
            continue;
        }

        if (line.Equals("exit", StringComparison.OrdinalIgnoreCase))
        {
            break;
        }

        var mainMenu = line.ToLowerInvariant();

        switch (mainMenu)
        {
            case "1":
                await CustomerMenuAsync();
                break;
            case "2":
                await ProductMenuAsync();
                break;
            case "3":
                await CategoryMenuAsync();
                break;
            case "4":
                await OrderMenuAsync();
                break;
            case "5":
                await SortOrderMenuAsync();
                break;
            default:
                Console.WriteLine("unknown command");
                break;
        }
}

static async Task CustomerMenuAsync()
{
    while (true)
    {
        Console.WriteLine("\nCustomer Menu: \n| 1.list customers | \n| 2.add customer | \n| 3.edit customer <id> | \n| 4.delete customer <id> | \n| 5.Number of Orders | \n| exit");
        Console.WriteLine("> ");
        var line = Console.ReadLine()?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(line))
        {
            continue;
        }

        if (line.Equals("exit", StringComparison.OrdinalIgnoreCase))
        {
            break;
        }

        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var customerMenu = parts[0].ToLowerInvariant();

        switch (customerMenu)
        {
            case "1":
                Console.WriteLine("Please enter page: ");
                var page = int.Parse(Console.ReadLine());
                Console.WriteLine("Please enter page size: ");
                var pageSize = int.Parse(Console.ReadLine());
                await CustomerServices.ListCustomerAsync(page, pageSize);
                break;
            case "2":
                await CustomerServices.AddCustomerAsync();
                break;
            case "3":
                if (parts.Length < 2 || !int.TryParse(parts[1], out var id))
                {
                    Console.WriteLine("Usage: 3 (Edit) <id>");
                    break;
                }
                await CustomerServices.EditCustomerAsync(id);
                break;
            case "4":
                if (parts.Length < 2 || !int.TryParse(parts[1], out var idD))
                {
                    Console.WriteLine("Usage: 4 (Delete) <id>");
                    break;
                }
                await CustomerServices.DeleteCustomerAsync(idD);
                break;
            case "5":
                await CustomerServices.NumberOfOrdersAsync();
                break;
            default:
                Console.WriteLine("unknown command");
                break;
        }
    }
}

static async Task ProductMenuAsync()
{
    while (true)
    {
        Console.WriteLine("\nProduct Menu: \n| 1.list products | \n| 2.list products by category <id> | \n| 3.add product | \n| 4.delete product <id> | \n| 5.edit product <id> | \n| 6.quantity sold | \n| exit");
        Console.WriteLine("> ");
        var line = Console.ReadLine()?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(line))
        {
            continue;
        }

        if (line.Equals("exit", StringComparison.OrdinalIgnoreCase))
        {
            break;
        }

        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var productMenu = parts[0].ToLowerInvariant();

        switch (productMenu)
        {
            case "1":
                await ProductServices.ListProductAsync();
                break;
            case "2":
            if (parts.Length < 2 || !int.TryParse(parts[1], out var idCategory))
            {
             Console.WriteLine("Usage: (2) Category Product <id>");
             break;
            }
            await ProductServices.ListProductsByCategoryAsync(idCategory);
            break;
            case "3":
                await ProductServices.AddProductAsync();
                break;
            case "4":
                if (parts.Length < 2 || !int.TryParse(parts[1], out var id))
                {
                    Console.WriteLine("Usage: (4) Edit product <id>");
                    break;
                }
                await ProductServices.EditProductAsync(id);
                break;
            case "5":
                if (parts.Length < 2 || !int.TryParse(parts[1], out var idD))
                {
                    Console.WriteLine("Usage: (5) Delete product <id>");
                    break;
                }
                await ProductServices.DeleteProductAsync(idD);
                break;
            case "6":
                await ProductServices.ProductSalesAsync();
                break;
            default:
                Console.WriteLine("unknown command");
                break;
        }
    }
}

    static async Task CategoryMenuAsync()
    {
        while (true)
        {
            Console.WriteLine("\nCategory Menu: \n| 1.list category | \n| 2.add category | \n| 3.edit category <id> | \n| 4.delete category <id> | \n| exit");
            Console.WriteLine("> ");
            var line = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            if (line.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                break; 
            }

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var categoryMenu = parts[0].ToLowerInvariant();

            switch (categoryMenu)
            {
                case "1":
                Console.WriteLine("Please enter page: ");
                var page = int.Parse(Console.ReadLine());
                Console.WriteLine("Please enter page size: ");
                var pageSize = int.Parse(Console.ReadLine());
                await CategoryServices.ListCategoryAsync(page, pageSize);
                break;
            case "2":
                    await CategoryServices.AddCategoryAsync();
                    break;
                case "3":
                    if (parts.Length < 2 || !int.TryParse(parts[1], out var id))
                    {
                        Console.WriteLine("Usage: Edit category <id>");
                        break;
                    }
                    await CategoryServices.EditCategoryAsync(id);
                    break;
                case "4":
                    if (parts.Length < 2 || !int.TryParse(parts[1], out var idD))
                    {
                        Console.WriteLine("Usage: Delete category <id>");
                        break;
                    }
                    await CategoryServices.DeleteCategoryAsync(idD);
                    break;
                default:
                    Console.WriteLine("unknown command");
                    break;
            }
        }
    }

    static async Task OrderMenuAsync()
    {
        while (true)
        {
            Console.WriteLine("\nOrder Menu: \n| 1.list orders | \n| 2.order details <id> | \n| 3.add order | \n| 4.order summary | \n| exit");
            Console.WriteLine("> ");
            var line = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            if (line.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var orderMenu = parts[0].ToLowerInvariant();

            switch (orderMenu)
            {
                case "1":
                    Console.WriteLine("Please enter page: ");
                    var page = int.Parse(Console.ReadLine());
                    Console.WriteLine("Please enter page size: ");
                    var pageSize = int.Parse(Console.ReadLine());
                    await OrderServices.ListOrderAsync(page, pageSize);
                    break;
                case "2":
                    if (parts.Length < 2 || !int.TryParse(parts[1], out var orderId))
                    {
                        Console.WriteLine("Usage: 2 (OrderDetails) <id>");
                        break;
                    }
                    await OrderServices.OrderDetailsAsync(orderId);
                    break;
                case "3":
                    await OrderServices.AddOrderAsync();
                    break;
                case "4":
                    await OrderServices.ListOrderSummary();
                    break;
                default:
                    Console.WriteLine("unknown command");
                    break;
            }
        }
    }

    static async Task SortOrderMenuAsync()
    {
        while (true)
        {
            Console.WriteLine("\nSort Order Menu: \n| 1. sort order by customer id <id> | \n| 2. sort order by status <Completed/Processing/Canceled> | \n| exit");
            Console.WriteLine("> ");
            var line = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            if (line.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var sortOrderMenu = parts[0].ToLowerInvariant();

            switch (sortOrderMenu)
            {
                case "1":
                    if (parts.Length < 2 || !int.TryParse(parts[1], out var customerId))
                    {
                        Console.WriteLine("Usage: 1 (Sort by Customer) <id>");
                        break;
                    }
                    await OrderServices.SortOrderCustomerAsync(customerId);
                    break;
                case "2":
                    if (!Enum.TryParse<Status>(parts[1], out var statusId))
                    {
                        Console.WriteLine("Usage: 2 (Sort by Status) <status>");
                        break;
                    }
                    await OrderServices.SortOrderStatusAsync(statusId);
                    break;
                default:
                    Console.WriteLine("unknown command");
                    break;
            }
        }
    }


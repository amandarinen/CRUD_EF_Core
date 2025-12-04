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
     Console.WriteLine("\nMain Menu: 1.customers | 2.products | 3.categories | 4.orders | 5.sortorders | exit");
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
        Console.WriteLine("\nCustomer Menu: 1.list customers | 2.add customer | 3.edit customer <id> | 4.delete customer <id> | exit");
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
                await CustomerServices.ListCustomerAsync();
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
        Console.WriteLine("\nProduct Menu: 1.list products | 2.list products by category <id> | 3.add product | 4.delete product <id> | 5.edit product <id> | exit");
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
            //case "categoryproduct":
            //if (parts.Length < 2 || !int.TryParse(parts[1], out var idCategory))
            //{
            // Console.WriteLine("Usage: CategoryProduct <id>");
            // break;
            //}
            //await ListProductsByCategoryAsync(idCategory);
            //break;
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
            Console.WriteLine("\nCategory Menu: 1.list category | 2.add category | 3.edit category <id> | 4.delete category <id> | exit");
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
                    await CategoryServices.ListCategoryAsync(); 
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
            Console.WriteLine("\nOrder Menu: 1.list orders | 2.order details <id> | 3.add order | 4.order summary | exit");
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
            Console.WriteLine("\nSort Order Menu: 1. sort order by customer id <id> | 2. sort order by status <Completed/Processing/Canceled> | exit");
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


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
     Console.WriteLine("\nCommands: customers | products | orders | sortorders | exit");
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

        var cmd = line.ToLowerInvariant();

        switch (cmd)
        {
            case "customers":
                await CustomerMenuAsync();
                break;
            //case "products":
            //await ProductMenuAsync();
            //break;
            case "orders":
                await OrderMenuAsync();
                break;
            case "sortorders":
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
            Console.WriteLine("\nCommands: 1.list customers | 2.add customer | 3.edit customer <id> | 4.delete customer <id> | exit");
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
            var cmd = parts[0].ToLowerInvariant();

            switch (cmd)
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

    static async Task OrderMenuAsync()
    {
        while (true)
        {
            Console.WriteLine("\nCommands: 1.list orders | 2.order details <id> | 3.add order | exit");
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
            var cmd = parts[0].ToLowerInvariant();

            switch (cmd)
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
            Console.WriteLine("\nCommands: 1. sort order by customer id <id> | 2. sort order by status <Completed/Processing/Canceled> | exit");
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
            var cmd = parts[0].ToLowerInvariant();

            switch (cmd)
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

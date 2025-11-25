using CRUD_EF_Core;
using CRUD_EF_Core.Models;
using CRUD_EF_Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


Console.WriteLine("DB: " + Path.Combine(AppContext.BaseDirectory, "shop.db"));
using (var db = new ShopContext())
{
    await db.Database.MigrateAsync();

    if (!await db.Customers.AnyAsync())
    {
        db.Customers.AddRange(
            new Customer { Name = "Anna Andersson", Email = "anna.andersson@gmail.com", City = "Stockholm" },
            new Customer { Name = "Peter Persson", Email = "peter@persson.com", City = "Malmö" },
            new Customer { Name = "Olof Olson", Email = "olof.olson@live.se", City = "Umeå" },
            new Customer { Name = "Gunilla Gran", Email = "gunillagran@outlook.com", City = "Göteborg" }
            );
        await db.SaveChangesAsync();
        Console.WriteLine("Seeded db with customers!");
    }

    if (!await db.Products.AnyAsync())
    {
        db.Products.AddRange(
            new Product { ProductName = "Headphones", ProductDescription = "Wireless headphones", ProductPrice = 799 },
            new Product { ProductName = "Keyboard", ProductDescription = "backlit mechanical keyboard", ProductPrice = 599 },
            new Product { ProductName = "Monitor", ProductDescription = "4K Display", ProductPrice = 349 },
            new Product { ProductName = "Smart Watch", ProductDescription = "Smart fitness watch", ProductPrice = 999 },
            new Product { ProductName = "Powerbank", ProductDescription = "Fast charging powerbank.", ProductPrice = 200 }
            );
        await db.SaveChangesAsync();
        Console.WriteLine("Seeded db with products!");
    }

    while (true)
    {
        Console.WriteLine("\nCommands: customers | products | orders | exit");
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
            //case "authors/books":
                //await OrderMenuAsync();
                //break;
            default:
                Console.WriteLine("unknown command");
                break;
        }
    }

    static async Task CustomerMenuAsync()
    {
        while (true)
        {
            Console.WriteLine("\nCommands: 1.listcustomers | 2.addcustomer | 3.editcustomer <id> | 4.deletecustomer <id> | exit");
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
                //case "4":
                    //await DeleteCustomerAsync();
                    //break;
                default:
                    Console.WriteLine("unknown command");
                    break;
            }
        }
    }
}
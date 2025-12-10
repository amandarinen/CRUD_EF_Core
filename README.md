# CRUD_EF_Core

A simple EF Core application demonstrating database structure, relations, CRUD operations and basic encryption.

This project was created for educational purposes as part of an introductory C# and .NET course.

It uses Entity Framework Core together with SQLite and features a console-based interface for managing customers, orders, products and categories.


## FEATURES

- **Full CRUD support:**
  Create, Read, Update and Delete customers, orders, products and categories.

- **User-friendly CLI:**
Clear, structured menus where the user navigates with numbers.

- **EF Core with SQLite:**
Includes models, DbContext configuration, relations, constraints and seeding.

- **Database Views & auto-calculated data:**
Demonstrates how to use EF Core keyless views for aggregated data (e.g. order count per customer, product sales).

- **Symmetric encryption example:**
Email addresses are encoded/decoded using simple XOR-based encryption.

- **Data seeding:**
The database is seeded with example customers, products, categories and orders.


## REQUIREMENTS 

Before running the project, install the following NuGet packages: 

Microsoft.EntityFrameworkCore.Sqlite (9.0.11)

Microsoft.EntityFrameworkCore.Design (9.0.11)


## RUNNING PROJECT

**Clone the repository:**
 
*git clone <https://github.com/amandarinen/CRUD_EF_Core>*

*cd CRUD_EF_Core*

*dotnet run*

**The CLI menu:**

Navigate using numbers (1–5) and type *exit* to close the program.

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD_EF_Core.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOrderSummary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"
            CREATE VIEW IF NOT EXISTS OrderSummary AS
            SELECT
                o.OrderId,
                o.OrderDate,
                c.Name AS CustomerName,
                c.Email AS CustomerEmail,
                IFNULL(SUM(orw.Quantity * orw.UnitPrice), 0) AS TotalAmount
            FROM Orders o
            JOIN Customers c ON c.CustomerId = o.CustomerId
            LEFT JOIN OrderRows orw On orw.OrderId = o.OrderId
            GROUP BY o.OrderId, o.OrderDate, c.Name, c.Email;
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"
            DROP VIEW IF EXISTS OrderSummary
            ");


        }
    }
}

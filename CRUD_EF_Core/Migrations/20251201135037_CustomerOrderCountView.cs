using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD_EF_Core.Migrations
{
    /// <inheritdoc />
    public partial class CustomerOrderCountView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE VIEW IF NOT EXISTS CostumerOrderCountView AS
            SELECT
                c.CustomerId,
                c.Name,
                c.Email,
                IFNULL(Count(o.OrderId), 0) AS NumberOfOrders
            FROM Customers c
            LEFT JOIN Orders o ON o.CustomerId = o.CustomerId
            GROUP BY c.CustomerId, c.Name, c.Email;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DROP VIEW IF EXISTS CostumerOrderCountView
            ");
        }
    }
}

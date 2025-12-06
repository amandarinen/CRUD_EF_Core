using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD_EF_Core.Migrations
{
    /// <inheritdoc />
    public partial class ProductSalesView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE VIEW IF NOT EXISTS ProductSalesView AS
            SELECT
                p.ProductId,
                p.ProductName,
                IFNULL(SUM(orw.Quantity), 0) AS TotalQuantitySold
            FROM Products p
            LEFT JOIN OrderRows orw ON orw.ProductId = p.ProductId
            GROUP BY p.ProductId, p.ProductName;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DROP VIEW IF EXISTS ProductSalesView");
        }
    }
}

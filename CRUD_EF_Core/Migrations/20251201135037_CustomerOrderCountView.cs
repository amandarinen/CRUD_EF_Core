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
                c.Name AS CustomerName,
                c.Email AS CustomerEmail,
                IFNULL(Count(o.OrderId), 0) AS NumberOfOrders
            FROM Customers c
            LEFT JOIN Orders o ON c.CustomerId = o.CustomerId
            GROUP BY c.CustomerId, c.Name, c.Email;
            ");

            //AFTER INSERT
            migrationBuilder.Sql(@"
            CREATE TRIGGER IF NOT EXISTS trg_Orders_Insert
            AFTER INSERT ON Orders
            BEGIN
                UPDATE Customers
                SET NumberOfOrders = (
                                    SELECT IFNULL(Count(o.OrderId), 0)
                                    FROM Orders 
                                    WHERE CustomerId = NEW.CustomerId
                                )
                                WHERE CustomerId = NEW.CustomerId;
                END;

            ");

            //AFTER UPDATE
            migrationBuilder.Sql(@"
            CREATE TRIGGER IF NOT EXISTS trg_Orders_Update
            AFTER UPDATE ON Orders
            BEGIN
                UPDATE Customers
                SET NumberOfOrders = (
                                    SELECT IFNULL(Count(o.OrderId), 0)
                                    FROM Orders 
                                    WHERE CustomerId = NEW.CustomerId
                                )
                                WHERE CustomerId = NEW.CustomerId;
                END;

            ");

            //AFTER DELETE
            migrationBuilder.Sql(@"
            CREATE TRIGGER IF NOT EXISTS trg_Orders_Delete
            AFTER DELETE ON Orders
            BEGIN
                UPDATE Customers
                SET NumberOfOrders = (
                                    SELECT IFNULL(Count(o.OrderId), 0)
                                    FROM Orders 
                                    WHERE CustomerId = NEW.CustomerId
                                )
                                WHERE CustomerId = NEW.CustomerId;
                END;

            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DROP VIEW IF EXISTS CostumerOrderCountView
            ");

            //migrationBuilder.Sql(@"
            //DROP TRIGGER IF EXISTS trg_Orders_Insert
            //");

            //migrationBuilder.Sql(@"
            //DROP TRIGGER IF EXISTS trg_Orders_Update
            //");

            //migrationBuilder.Sql(@"
            //DROP TRIGGER IF EXISTS trg_Orders_Delete
            //");
        }
    }
}

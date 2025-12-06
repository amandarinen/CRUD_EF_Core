using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD_EF_Core.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //AFTER INSERT
            migrationBuilder.Sql(@"
            CREATE TRIGGER IF NOT EXISTS trg_OrderRow_Insert
            AFTER INSERT ON OrderRows
            BEGIN
                UPDATE Orders
                SET TotalAmount = (
                                    SELECT IFNULL(SUM(Quantity * UnitPrice), 0) 
                                    FROM OrderRows 
                                    WHERE OrderId = NEW.OrderId
                                )
                                WHERE OrderId = NEW.OrderId;
                END;

            ");

            //AFTER UPDATE
            migrationBuilder.Sql(@"
            CREATE TRIGGER IF NOT EXISTS trg_OrderRow_Update
            AFTER UPDATE ON OrderRows
            BEGIN
                UPDATE Orders
                SET TotalAmount = (
                                    SELECT IFNULL(SUM(Quantity * UnitPrice), 0) 
                                    FROM OrderRows 
                                    WHERE OrderId = NEW.OrderId
                                )
                                WHERE OrderId = NEW.OrderId;
                END;

            ");

            //AFTER DELETE
            migrationBuilder.Sql(@"
            CREATE TRIGGER IF NOT EXISTS trg_OrderRow_Delete
            AFTER DELETE ON OrderRows
            BEGIN
                UPDATE Orders
                SET TotalAmount = (
                                    SELECT IFNULL(SUM(Quantity * UnitPrice), 0) 
                                    FROM OrderRows 
                                    WHERE OrderId = OLD.OrderId
                                )
                                WHERE OrderId = OLD.OrderId;
                END;

            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DROP TRIGGER IF EXISTS trg_OrderRow_Insert
            ");

            migrationBuilder.Sql(@"
            DROP TRIGGER IF EXISTS trg_OrderRow_Update
            ");

            migrationBuilder.Sql(@"
            DROP TRIGGER IF EXISTS trg_OrderRow_Delete
            ");
        }
    }
}

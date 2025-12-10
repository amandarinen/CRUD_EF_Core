using System.ComponentModel.DataAnnotations;

namespace CRUD_EF_Core.Models
{
    /// <summary>
    /// Represents a single row/item in an order.
    /// </summary>
    public class OrderRow
    {
        // PK, the unique identifier for the order row.
        public int OrderRowId { get; set; }

        // FK, the order this row belongs to.
        public int OrderId { get; set; }

        // FK, the product associated with this row.
        public int ProductId { get; set; }

        // Quantity of the product in the order row.
        [Required]
        public int Quantity { get; set; }

        // Price per product unit at the time of purchase.
        [Required]
        public decimal UnitPrice { get; set; }

        // Reference to the order.
        public Order? Order { get; set; }

        // Reference to the product for this order row.
        public Product? Product { get; set; }
    }
}
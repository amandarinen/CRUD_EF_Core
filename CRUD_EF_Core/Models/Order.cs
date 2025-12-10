using System.ComponentModel.DataAnnotations;

namespace CRUD_EF_Core.Models
{
    /// <summary>
    /// Represents the status of an order.
    /// </summary>
    public enum Status
    {
        Processing,
        Completed,
        Canceled
    }

    /// <summary>
    /// Represents a customer order.
    /// </summary>
    public class Order
    {
        // PK, the unique identifier for the order.
        public int OrderId { get; set; }

        // FK, the customer associated with the order.
        public int CustomerId { get; set; }

        // Date the order was created.
        public DateTime OrderDate { get; set; }

        // Status of the order.
        [Required, MaxLength(100)]
        public Status Status { get; set; }

        // Total amount for the order.
        [Required]
        public decimal TotalAmount { get; set; }

        // Reference to the customer that owns the order.
        public Customer? Customer { get; set; }

        // Collection of order rows belonging to this order.
        public List<OrderRow> OrderRows { get; set; } = new();
    }
}

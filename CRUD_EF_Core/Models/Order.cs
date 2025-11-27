using System.ComponentModel.DataAnnotations;

namespace CRUD_EF_Core.Models
{
    public enum Status
    {
        Processing,
        Completed,
        Canceled
    }

    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }

        [Required, MaxLength(100)]
        public Status Status { get; set; }

        [Required]
        public decimal TotalAmount => OrderRows.Sum(amount => amount.UnitPrice* amount.Quantity);
        public Customer? Customer { get; set; }
        public List<OrderRow> OrderRows { get; set; } = new();
    }
}

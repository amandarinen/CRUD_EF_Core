using System.ComponentModel.DataAnnotations;

namespace CRUD_EF_Core.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }

        [Required, MaxLength(100)]
        public string Status { get; set; } = null!;

        [Required]
        public decimal TotalAmount { get; set; }
        public Customer? Customer { get; set; }
        public List<OrderRow> OrderRows { get; set; } = new();
    }
}

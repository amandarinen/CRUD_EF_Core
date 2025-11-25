using System.ComponentModel.DataAnnotations;

namespace CRUD_EF_Core.Models
{
    public class OrderRow
    {
        public int OrderRowId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }
}

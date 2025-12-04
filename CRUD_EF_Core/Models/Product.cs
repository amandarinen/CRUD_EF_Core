using System.ComponentModel.DataAnnotations;

namespace CRUD_EF_Core.Models
{
    public class Product
    {
        //PK
        public int ProductId { get; set; }

        [Required]
        public decimal ProductPrice { get; set; }

        [Required, MaxLength(100)]
        public string ProductName { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? ProductDescription { get; set; }

        //FK
        public int? CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}

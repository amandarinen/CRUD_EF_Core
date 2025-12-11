using System.ComponentModel.DataAnnotations;

namespace CRUD_EF_Core.Models
{
    /// <summary>
    /// Represents a product available in the store.
    /// </summary>
    public class Product
    {
        // PK, the unique identifier for the product.
        public int ProductId { get; set; }

        [Required]
        public decimal ProductPrice { get; set; }

        [Required, MaxLength(100)]
        public string ProductName { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? ProductDescription { get; set; }

        // FK, the category this product belongs to.
        public int? CategoryId { get; set; }

        // Reference to the category.
        public Category? Category { get; set; }
    }
}
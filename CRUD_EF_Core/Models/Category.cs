using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_EF_Core.Models
{
    /// <summary>
    /// Represents a product category.
    /// </summary>
    public class Category
    {
        //PK, the unique identifier for category
        public int CategoryId { get; set; }

        [Required, MaxLength(100)]
        public string CategoryName { get; set; } = null!;

        [MaxLength(250)]
        public string? CategoryDescription { get; set; }

        // A collection of products that belong to this category.
        public List<Product> Products { get; set; } = new();
    }
}

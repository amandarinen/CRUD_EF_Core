using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_EF_Core.Models
{
    public class Category
    {
        //PK
        public int CategoryId { get; set; }

        [Required, MaxLength(100)]
        public string CategoryName { get; set; } = null!;

        [MaxLength(250)]
        public string? CategoryDescription { get; set; }

        public List<Product> Products { get; set; } = new();
    }
}

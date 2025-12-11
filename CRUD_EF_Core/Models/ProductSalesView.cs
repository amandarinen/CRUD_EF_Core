using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_EF_Core.Models
{
    /// <summary>
    /// Represents a keyless entity mapped to the SQL view ProductSalesView,
    /// providing a read-only summary of the total quantity sold for each product.
    /// </summary>
    [Keyless]
    public class ProductSalesView
    {
        // PK, the unique identifier for the product.
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int TotalQuantitySold { get; set; }
    }
}
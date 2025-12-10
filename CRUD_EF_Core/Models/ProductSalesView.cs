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
        // The unique identifier for the product.
        public int ProductId { get; set; }

        // Name of product.
        public string ProductName { get; set; } = string.Empty;

        // The total number of units sold.
        public int TotalQuantitySold { get; set; }
    }
}
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_EF_Core.Models
{
    /// <summary>
    /// Represents a keyless entity mapped to the SQL view OrderSummary,
    /// used to provide a read-only overview of orders.
    /// </summary>
    [Keyless]
    public class OrderSummary
    {
        // The unique identifier from Order.
        public int OrderId { get; set; }

        // The date when the order was created.
        public DateTime OrderDate { get; set; }

        // Name of customer
        public string CustomerName { get; set; } = string.Empty;

        // Customer email
        public string CustomerEmail { get; set; } = string.Empty;

        // The total amount for the order, calculated from the order rows.
        public decimal TotalAmount { get; set; }
    }
}
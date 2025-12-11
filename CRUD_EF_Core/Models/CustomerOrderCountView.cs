using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_EF_Core.Models
{
    /// <summary>
    /// Represents a keyless entity mapped to the SQL view CustomerOrderCountView,
    /// providing a read-only summary of how many orders each customer has made.
    /// </summary>
    [Keyless]
    public class CustomerOrderCountView
    {
        // The unique identifier for the customer.
        public int CustomerId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        // The total number of orders placed by the customer.
        public int NumberOfOrders { get; set; }
    }
}
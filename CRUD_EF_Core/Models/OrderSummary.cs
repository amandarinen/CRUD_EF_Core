using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_EF_Core.Models
{
    // detta är en keyless  entitet (INGEN PK)
    //den representerar en SQL View, en spara-select query
    // vi använder dessa views i EF Core som gör att den kan läsa den precis som en vanloig tabell

    [Keyless] //frivilligt!
    public class OrderSummary
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
    }
}

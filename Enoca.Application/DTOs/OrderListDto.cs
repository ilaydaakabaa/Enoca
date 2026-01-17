using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enoca.Application.DTOs
{
    public class OrderListDto
    {
        public int OrderId { get; set; }
        public int CarrierId { get; set; }
        public string CarrierName { get; set; } = string.Empty;
        public int OrderDesi { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderCarrierCost { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enoca.Domain.Entities
{
    public  class Order
    {
        public int OrderId { get; set; }

        public int CarrierId { get; set; }
        public Carrier Carrier { get; set; } = null!;

        public int OrderDesi { get; set; }
        public DateTime OrderDate { get; set; }

        public decimal OrderCarrierCost { get; set; }
    }
}

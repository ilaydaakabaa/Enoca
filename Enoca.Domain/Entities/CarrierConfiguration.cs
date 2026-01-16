using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enoca.Domain.Entities
{
     public  class CarrierConfiguration
    {
        public int CarrierConfigurationId { get; set; }

        public int CarrierId { get; set; }
        public Carrier Carrier { get; set; } = null!;

        public int CarrierMaxDesi { get; set; }
        public int CarrierMinDesi { get; set; }

        public decimal CarrierCost { get; set; }
    }
}

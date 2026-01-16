using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enoca.Domain.Entities
{
    public class Carrier
    {
        public int CarrierId { get; set; }
        public string CarrierName { get; set; } = string.Empty;
        public bool CarrierIsActive { get; set; }
        public int CarrierPlusDesiCost { get; set; }

        public ICollection<CarrierConfiguration> CarrierConfigurations { get; set; } = new List<CarrierConfiguration>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}

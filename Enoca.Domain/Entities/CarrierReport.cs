using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enoca.Domain.Entities
{
    public class CarrierReport
    {
        public int CarrierReportId { get; set; }
        public int CarrierId { get; set; }
        public decimal CarrierCost { get; set; }
        public DateTime CarrierReportDate { get; set; }

        public Carrier? Carrier { get; set; }
    }
}

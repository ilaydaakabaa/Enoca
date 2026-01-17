using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enoca.Application.DTOs
{
    public class CarrierConfigurationUpdateDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "CarrierId geçerli olmalıdır.")]
        public int CarrierId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "CarrierMaxDesi 1 veya daha büyük olmalıdır.")]
        public int CarrierMaxDesi { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "CarrierMinDesi 1 veya daha büyük olmalıdır.")]

        public int CarrierMinDesi { get; set; }

       
        public decimal CarrierCost { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enoca.Application.DTOs
{
    public  class CarrierCreateDto
    {
        [Required(ErrorMessage = "CarrierName zorunludur.")]
        [StringLength(200, ErrorMessage = "CarrierName en fazla 200 karakter olabilir.")]
        public string CarrierName { get; set; } = string.Empty;
        public bool CarrierIsActive { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "CarrierPlusDesiCost 1 veya daha büyük olmalıdır.")]
        public int CarrierPlusDesiCost { get; set; }
    }
}

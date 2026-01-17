using System.ComponentModel.DataAnnotations;

namespace Enoca.Application.DTOs;

public class CarrierConfigurationCreateDto
{
    [Range(1, int.MaxValue, ErrorMessage = "CarrierId geçerli olmalıdır.")]
    public int CarrierId { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "CarrierMaxDesi 1 veya daha büyük olmalıdır.")]
    public int CarrierMaxDesi { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "CarrierMinDesi 1 veya daha büyük olmalıdır.")]
    public int CarrierMinDesi { get; set; }
    
    public decimal CarrierCost { get; set; }
}

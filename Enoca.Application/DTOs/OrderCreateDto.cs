

using System.ComponentModel.DataAnnotations;

namespace Enoca.Application.DTOs
{
    public class OrderCreateDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "OrderDesi 1 veya daha büyük olmalıdır.")]
        public int OrderDesi { get; set; }
        [Required(ErrorMessage = "OrderDate zorunludur.")]
        public DateTime OrderDate { get; set; }
    }
}
